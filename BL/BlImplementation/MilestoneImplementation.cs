﻿using BlApi;
using BO;
using DalApi;
using System;
using System.ComponentModel.Design.Serialization;

namespace BlImplementation;

internal class MilestoneImplementation : IMilestone
{
    private DalApi.IDal _dal = Factory.Get;

    public IEnumerable<DO.Dependency> Create()
    {
        var groupedDependencies = _dal.Dependency.ReadAll()
            .GroupBy(d => d.DependentTask)
            .OrderBy(group => group.Key)
            .Select(group => (group.Key, group.Select(d => d!.DependsOnTask).ToList()))
            .ToList();

        var uniqueLists = groupedDependencies
     .Select(group => group.Item2.ToList())
     .DistinctBy(list => string.Join(",", list))
     .ToList();

        int milestoneAlias = 1;

        List<DO.Dependency> dependencies = new List<DO.Dependency>();

        foreach (var tasksList in uniqueLists)
        {
            if (tasksList != null)
            {
                DO.Task doTask = new DO.Task
                    (0,
                    $"a milestone with Id: {milestoneAlias}",
                    $"M{milestoneAlias}",
                    true,
                    DateTime.Now,
                    null, null, true, null, null, null, null, null, null, null);
                try
                {
                    int milestoneId = _dal.Task.Create(doTask);

                    foreach (var taskId in tasksList)
                    {
                        dependencies.Add(new DO.Dependency
                        {
                            DependentTask = milestoneId,
                            DependsOnTask = taskId
                        });
                    }

                    foreach (var dependencyGroup in groupedDependencies)
                    {
                        if (dependencyGroup.Item2.SequenceEqual(tasksList))
                        {
                            dependencies.Add(new DO.Dependency
                            {
                                DependentTask = dependencyGroup.Item1,
                                DependsOnTask = milestoneId
                            });
                        }
                    }

                    milestoneAlias++;
                }
                catch (DO.DalAlreadyExistsException ex)
                {
                    throw new BO.BlFailedToCreate($"failed to create Milestone with Alias = M{milestoneAlias}", ex);
                }
            }
        }

        //משימות שלא תלויות בשום משימה
        var independentOnTasks = _dal.Task.ReadAll(t=>!t.Milestone)
    .Where(task => !dependencies.Any(d => d.DependentTask == task!.Id))
    .Select(task => task!.Id)
    .ToList();

        DO.Task startMilestone = new DO.Task
               (0,
               $"a milestone with Id: {0}",
               $"Start",
               true,
               DateTime.Now,
               null, null, true, null, null, null, null, null, null, null);

        //משימות ששום משימה לא תלויה בהן
        var independentTasks = _dal.Task.ReadAll(t=>!t.Milestone)
    .Where(task => !dependencies.Any(d => d.DependsOnTask == task!.Id))
    .Select(task => task!.Id)
    .ToList();

        DO.Task endMilestone = new DO.Task
               (0,
               $"a milestone with Id: {milestoneAlias}",
               $"End",
               true,
               DateTime.Now,
               null, null, true, null, null, null, null, null, null, null);

        _dal.Dependency.ReadAll().ToList().ForEach(d => _dal.Dependency.Delete(d!.Id));

        try
        {
            int startMilestoneId = _dal.Task.Create(startMilestone);
            int endMilestoneId = _dal.Task.Create(endMilestone);

            foreach (var task in independentOnTasks)
            {
                dependencies.Add(new DO.Dependency
                {
                    DependentTask = task,
                    DependsOnTask = startMilestoneId
                });
            }

            foreach (var task in independentTasks)
            {
                dependencies.Add(new DO.Dependency
                {
                    DependentTask = endMilestoneId,
                    DependsOnTask = task
                });
            }
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlFailedToCreate("Failed to create END or START milestone", ex);
        }

        foreach (var dependency in dependencies.ToList())
        {
            if (dependency != null)
                _dal.Dependency.Create(dependency);
        }

        //CalculateDatesInOrder();

        return _dal.Dependency.ReadAll()!;
    }

    public Milestone? Read(int id)
    {
        try
        {
            DO.Task? doTaskMilestone = _dal.Task.Read(t => t.Id == id && t.Milestone);
            if (doTaskMilestone == null)
                throw new BO.BlDoesNotExistException($"Milstone with ID={id} does Not exist");

            var tasksId = _dal.Dependency.ReadAll(d => d.DependentTask == doTaskMilestone.Id)
                                         .Select(d => d.DependsOnTask);
            var tasks = _dal.Task.ReadAll(t => tasksId.Contains(t.Id)).ToList();

            var tasksInList = tasks.Select(t => new BO.TaskInList
            {
                Id = t.Id,
                Description = t.Description,
                Alias = t.Alias,
                Status = Tools.CalculateStatus(t.Start, t.ForecastDate, t.Deadline, t.Complete)
            }).ToList();

            double CompletionPercentage = 0;
            if(tasksInList.Count > 0)
            {
                CompletionPercentage = (tasksInList.Count(t => t.Status == Status.OnTrack) / tasksInList.Count * 0.1) * 100;
            }

            return new BO.Milestone()
            {
                Id = doTaskMilestone.Id,
                Description = doTaskMilestone.Description,
                Alias = doTaskMilestone.Alias,
                CreateAt = doTaskMilestone.CreateAt,
                Status = Tools.CalculateStatus(doTaskMilestone.Start, doTaskMilestone.ForecastDate, doTaskMilestone.Deadline, doTaskMilestone.Complete),
                ForecastDate = doTaskMilestone.ForecastDate,
                Deadline = doTaskMilestone.Deadline,
                Complete = doTaskMilestone.Complete,
                CompletionPercentage = CompletionPercentage,
                Remarks = doTaskMilestone.Remarks,
                Dependencies = tasksInList!
            };
        }
        catch (Exception ex)
        {
            throw new BlFailedToRead("Failed to build milestone ", ex);
        }
    }

    public void Update(BO.Milestone item)
    {
        Tools.ValidatePositiveNumber(item.Id, nameof(item.Id));
        Tools.ValidateNonEmptyString(item.Alias, nameof(item.Alias));

        try
        {
            DO.Task oldDoTask = _dal.Task.Read(t => t.Id == item.Id)!;
            DO.Task doTask = new DO.Task(item.Id, item.Description, item.Alias, false, item.CreateAt, null, null, true, oldDoTask.Start, item.ForecastDate, item.Deadline, item.Complete, oldDoTask.Deliverables, item.Remarks, null);
            _dal.Task.Update(doTask);
        }
        catch (DO.DalAlreadyExistsException ex)
        {
            throw new BO.BlAlreadyExistsException($"Milsetone with ID={item.Id} not exists", ex);
        }
    }

    private void CalculateDatesOfTasksAndMilestones()
    {
        var lastMilestone = _dal.Task.Read(t => t.Alias == "End");

        if (lastMilestone == null)
            return;

        recursionDatesForNilstones(lastMilestone);
    }

    private void recursionDatesForNilstones(DO.Task milestone)
    {
        if (milestone.Deadline!=null) return;

        DateTime? date=CalculateLatestFinishDate(milestone);

        _dal.Task.Update(new DO.Task(
           milestone.Id,
           milestone.Description,
           milestone.Alias,
           true,
           milestone.CreateAt,
           null,
           null,
           true,
           null,
           null,
           date,
           null,
           null,
           null,
           null));

        var dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == milestone.Id);

        if (dependencies == null)
            return;


        var depenntOnTasks = dependencies.Select(d => _dal.Task.Read(t => t.Id == d.DependsOnTask));

        foreach (var task in depenntOnTasks)
        {
            _dal.Task.Update(
                       new DO.Task(
               task!.Id,
               task.Description,
               task.Alias,
               task.Milestone,
               task.CreateAt,
               task.RequiredEffortTime,
               task.Level,
               task.IsActive,
               task.Start,
               (DateTime)(date) - (TimeSpan)(task.RequiredEffortTime),
               date,
               task.Complete,
               task.Deliverables,
               task.Remarks,
               task.EngineerId));

        }
    }

    private DateTime? CalculateLatestFinishDate(DO.Task task)
    {
        var dependencies = _dal.Dependency.ReadAll(d => d.DependsOnTask == task.Id);

        // אם אין למשימה תלות, התאריך האחרון האפשרי הוא תאריך הסיום המתוכנן של הפרויקט
        if (dependencies == null)
            return _dal.deadlineProject;

        // קביעת תאריך הסיום האחרון האפשרי
        var dependenciesIds = dependencies.Select(d => d.DependentTask).ToList();
        var dependentsTask = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
        DateTime? latestFinishDate = dependentsTask.Max(t => {
            return (DateTime)(t.Deadline) - (TimeSpan)(t.RequiredEffortTime);
        });
        return latestFinishDate;
    }

    //private void CalculateDatesInOrder()
    //{
    //    var allMilestones = _dal.Task.ReadAll(t => t.Milestone).OrderBy(milestone => milestone.Alias).Reverse().ToList();

    //    foreach (var milstone in allMilestones)
    //    {
    //        _dal.Task.Update(new DO.Task(
    //        milstone.Id,
    //        milstone.Description,
    //        milstone.Alias,
    //        true,
    //        milstone.CreateAt,
    //        null,
    //        null,
    //        true,
    //        null,
    //        null,
    //        CalculateLatestFinishDate(milstone),
    //        null,
    //        null,
    //        null,
    //        null));

    //        var dependentOnTasksDependency = _dal.Dependency.ReadAll(d => d.DependentTask == milstone.Id);
    //        if (dependentOnTasksDependency != null)
    //        {
    //            var dependentOnTasks = _dal.Task.ReadAll(t => dependentOnTasksDependency.Any(d => d.DependsOnTask == t.Id));
    //            foreach (var task in dependentOnTasks)
    //            {
    //                _dal.Task.Update(
    //                    new DO.Task(
    //            task!.Id,
    //            task.Description,
    //            task.Alias,
    //            task.Milestone,
    //            task.CreateAt,
    //            task.RequiredEffortTime,
    //            task.Level,
    //            task.IsActive,
    //            task.Start,
    //            (DateTime)(milstone.Deadline) - (TimeSpan)(task.RequiredEffortTime),
    //            milstone.Deadline,
    //            task.Complete,
    //            task.Deliverables,
    //            task.Remarks,
    //            task.EngineerId));
    //            }
    //        }
    //    }

    //    allMilestones.Reverse();
    //    var reverseMilestone = allMilestones.ToList();

    //    foreach (var milestone in allMilestones)
    //    {
    //        _dal.Task.Update(new DO.Task(
    //            milestone!.Id,
    //        milestone.Description,
    //        milestone.Alias,
    //        true,
    //        milestone.CreateAt,
    //        null,
    //        null,
    //        true,
    //        null,
    //        CalculateEarliestStartDate(milestone),
    //        milestone.Deadline,
    //        null,
    //        null,
    //        null,
    //        null));
    //    }
    //}



    //private DateTime? CalculateEarliestStartDate(DO.Task milestone)
    //{
    //    var dependencies = _dal.Dependency.ReadAll(d => d.DependentTask == milestone.Id);

    //    // אם אין למשימה תלות, התאריך הראשון האפשרי הוא תאריך ההתחלה המתוכנן של הפרויקט
    //    if (dependencies == null)
    //        return _dal.startProject;

    //    // קביעת תאריך התחלה הראשון האפשרי
    //    var dependenciesIds = dependencies.Select(d => d.DependsOnTask).ToList();
    //    var dependentsTask = _dal.Task.ReadAll(t => dependenciesIds.Any(number => number == t.Id)).ToList();
    //    DateTime? earliestStartDate = dependentsTask.Min(t =>
    //    {
    //        return t.ForecastDate;
    //    });
    //    return earliestStartDate;
    //}
}