﻿using BO;
using DalApi;
using DalTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BlTest
{
    internal class Program
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
        enum MainMenu { EXIT, MILESTONE, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }

        public static void MilestoneMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, ,UPDATE }");
                int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1:
                        try
                        {
                            s_bl.Milestone.Create();
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToCreate("Failed to create milestone ", ex);
                        }
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out id);
                        try
                        {
                            if (s_bl.Milestone!.Read(id) is null)
                                Console.WriteLine("no milestone's task found");
                            else
                            {
                                Console.WriteLine(s_bl.Milestone!.Read(id)!.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToRead("Failed to read milestone ", ex);
                        }
                        break;
                    case 3:
                        int idMilestoneUpdate;
                        string milestoneDescriptionUpdate,
                            milestoneAliasUpdate;
                        string? milestoneRemarksUpdate;
                        Console.WriteLine("Enter id for reading milestone");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out idMilestoneUpdate);
                        try
                        {
                            Milestone updatedMilestone = s_bl.Milestone.Read(idMilestoneUpdate)!;
                            Console.WriteLine(updatedMilestone.ToString());
                            Console.WriteLine("Enter description, alias, remarks ");//if null to put the same details
                            milestoneDescriptionUpdate = Console.ReadLine() ?? updatedMilestone.Description;
                            milestoneAliasUpdate = Console.ReadLine() ?? updatedMilestone.Alias;
                            milestoneRemarksUpdate = Console.ReadLine() ?? updatedMilestone.Remarks;
                            BO.Milestone newMilUpdate = new BO.Milestone()
                            {
                                Id = idMilestoneUpdate,
                                Description = milestoneDescriptionUpdate,
                                Alias = milestoneAliasUpdate,
                                CreateAt = s_bl.Milestone.Read(idMilestoneUpdate)!.CreateAt,
                                Status = s_bl.Milestone.Read(idMilestoneUpdate)!.Status,
                                ForecastDate = s_bl.Milestone.Read(idMilestoneUpdate)!.ForecastDate,
                                Deadline = s_bl.Milestone.Read(idMilestoneUpdate)!.Deadline,
                                Complete = s_bl.Milestone.Read(idMilestoneUpdate)!.Complete,
                                CompletionPercentage = s_bl.Milestone.Read(idMilestoneUpdate)!.CompletionPercentage,
                                Remarks = milestoneRemarksUpdate,
                                Dependencies = s_bl.Milestone.Read(idMilestoneUpdate)!.Dependencies
                            };
                            try
                            {
                                s_bl.Milestone.Update(newMilUpdate);
                            }
                            catch (Exception ex)
                            {
                                throw new BlFailedToCreate($"failed to update milestone id={idMilestoneUpdate}", ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToRead($"failed to read id: {idMilestoneUpdate} of engineer", ex);
                        }
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 3);

        }
        public static void EngineerMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter id, name,isactive, email, level, cost and role");
                        int idEngineer,
                            idTask;
                        string nameEngineer,
                               emailEngineer,
                               inputEE,
                               inputR; ;
                        DO.EngineerExperience levelEngineer;
                        DO.Roles role;
                        bool isActive;
                        double costEngineer;
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idEngineer);
                        nameEngineer = (Console.ReadLine()!);
                        isActive = Console.ReadLine() == "false" ? false : true;
                        emailEngineer = Console.ReadLine()!;
                        inputEE = Console.ReadLine()!;
                        inputR = Console.ReadLine()!;
                        levelEngineer = (DO.EngineerExperience)Enum.Parse(typeof(DO.EngineerExperience), inputEE);
                        double.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a doublenumber please"), out costEngineer);
                        role = (DO.Roles)Enum.Parse(typeof(DO.Roles), inputR);
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idTask);
                        BO.Engineer newEng = new BO.Engineer()
                        {
                            Id = idEngineer,
                            Name = nameEngineer,
                            IsActive = isActive,
                            Email = emailEngineer,
                            Level = (BO.EngineerExperience)levelEngineer,
                            Cost = costEngineer,
                            Role = (BO.Roles)role,
                            Task = new BO.TaskInEngineer()
                            {
                                Id = idTask,
                                Alias = s_bl.Task.Read(idTask)!.Alias
                            }
                        };
                        try
                        {
                            s_bl.Engineer.Create(newEng);
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToCreate("Failed to build engineer ", ex);
                        }
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out id);
                        try
                        {
                            if (s_bl.Engineer!.Read(id) is null)
                                Console.WriteLine("no engineer found");
                            else
                            {
                                Console.WriteLine(s_bl.Engineer!.Read(id)!.ToString());
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToRead("Failed to read engineer ", ex);
                        }

                        break;
                    case 3:
                        try
                        {
                            s_bl.Engineer!.ReadAll()
                            .ToList()
                            .ForEach(engineer => Console.WriteLine(engineer.ToString()));
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToReadAll("Failed to readall engineer ", ex);
                        }
                        break;
                    case 4:
                        int idEngineerUpdate,
                            idTaskUpdate;
                        string? nameEngineerUpdate;
                        string emailEngineerUpdate,
                            inputUpdateEE,
                            inputUpdateR;
                        EngineerExperience levelEngineerUpdate;
                        double costEngineerUpdate;
                        bool isActiveUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine()!);
                        try
                        {
                            Engineer updatedEngineer = s_bl.Engineer.Read(idEngineerUpdate)!;
                            Console.WriteLine(updatedEngineer.ToString());
                            Console.WriteLine("Enter name, isactive,level,cost,role and id of task to update");//if null to put the same details
                            nameEngineerUpdate = Console.ReadLine() ?? updatedEngineer.Name;
                            isActiveUpdate = Console.ReadLine() == "false" ? false : true;
                            emailEngineerUpdate = Console.ReadLine() ?? updatedEngineer.Email;
                            inputUpdateEE = Console.ReadLine()!;
                            inputUpdateR = Console.ReadLine()!;
                            levelEngineerUpdate = string.IsNullOrWhiteSpace(inputUpdateEE) ? updatedEngineer.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputUpdateEE);
                            double.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out costEngineerUpdate);
                            role = (DO.Roles)Enum.Parse(typeof(DO.Roles), inputUpdateR);
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idTaskUpdate);
                            BO.Engineer newEngUpdate = new BO.Engineer()
                            {
                                Id = idEngineerUpdate,
                                Name = nameEngineerUpdate,
                                IsActive = isActiveUpdate,
                                Email = emailEngineerUpdate,
                                Level = (BO.EngineerExperience)levelEngineerUpdate,
                                Cost = costEngineerUpdate,
                                Role = (BO.Roles)role,
                                Task = new BO.TaskInEngineer()
                                {
                                    Id = idTaskUpdate,
                                    Alias = s_bl.Task.Read(idTaskUpdate)!.Alias
                                }
                            };
                            try
                            {
                                s_bl.Engineer.Update(newEngUpdate);
                            }
                            catch (Exception ex)
                            {
                                throw new BlFailedToCreate($"failed to update engineer id={idEngineerUpdate}", ex);
                            }
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToRead($"failed to read id: {idEngineerUpdate} of engineer", ex);
                        }
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idDelete);
                        try
                        {
                            s_bl.Engineer!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToDelete("failed to delete engineer", ex);
                        }

                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        private static void TaskMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out chooseSubMenu);
                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter  description, alias,deriverables, remarks,milestone, dates and task's level");
                        int  days,
                            milestoneInTaskId,
                            engineerInTaskId,
                            taskInListId;
                        string taskDescription,
                               taskAlias,
                               taskDeliverables,
                               taskRemarks,
                               inputEE;
                        bool isActive;
                        TimeSpan requiredEffortTime;
                        EngineerExperience taskLevel;
                        List<BO.TaskInList?> taskInList = new List<TaskInList?>();
                        Console.WriteLine("Enter  description");
                        taskDescription = Console.ReadLine()!;
                        Console.WriteLine("Enter  alias");
                        taskAlias = Console.ReadLine()!;
                        isActive = true;
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskCreateAt);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskStart);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskForecastDate);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskDeadline);
                        //DateTime.TryParse(Console.ReadLine() ?? throw new Exception("enter a date please"), out taskComplete);
                        Console.WriteLine("Enter  deliverables");
                        taskDeliverables = Console.ReadLine()!;
                        Console.WriteLine("Enter  required effort time");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out days);
                        requiredEffortTime = TimeSpan.FromDays(days);
                        Console.WriteLine("Enter remarks");
                        taskRemarks = Console.ReadLine()!;
                        Console.WriteLine("Enter engineer id");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out engineerInTaskId);
                        Console.WriteLine("Enter input for level");
                        inputEE = Console.ReadLine()!;
                        taskLevel = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputEE);
                        Console.WriteLine("Enter task in list id");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out taskInListId);
                        while (taskInListId != -1)
                        {
                            taskInList!.Add(new BO.TaskInList()
                            {
                                Id = taskInListId,
                                Description = s_bl.Task.Read(taskInListId)!.Description,
                                Alias = s_bl.Task.Read(taskInListId)!.Alias,
                                Status = BO.Helper.CalculateStatus(taskStart, taskForecastDate, taskDeadline, taskComplete)
                            });
                            taskInListId = int.Parse(Console.ReadLine()!);
                        }
                        BO.Task newTask = new BO.Task()
                        {
                            Id = 0,
                            Description = taskDescription,
                            Alias = taskAlias,
                            IsActive = isActive,
                            CreateAt = DateTime.Now,
                            Start = null,
                            ForecastDate = null,
                            Deadline = null,
                            Complete = null,
                            Deliverables = taskDeliverables,
                            RequiredEffortTime = requiredEffortTime,
                            Remarks = taskRemarks,
                            Engineer = new EngineerInTask()
                            {
                                Id = engineerInTaskId,
                                Name = s_bl.Engineer.Read(engineerInTaskId)!.Name!
                            },
                            Level = taskLevel,
                            Status = Tools.CalculateStatus(null, null, null, null),
                            Milestone = null,
                            Dependencies = taskInList!
                        };
                        try { s_bl.Task.Create(newTask); }
                        catch (Exception ex) { throw new BlFailedToCreate("failed to create task", ex); }
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out id);
                        try
                        {
                            if (s_bl.Task!.Read(id) is null)
                                Console.WriteLine("no task found");
                            else
                                Console.WriteLine(s_bl.Task!.Read(id)!.ToString());
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToRead($"failed to read id: {id} in task", ex);
                        }
                        break;
                    case 3:
                        try
                        {
                            s_bl.Task!.ReadAll()
                                      .ToList()
                                      .ForEach(task => Console.WriteLine(task.ToString()));
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToRead($"failed to readall task", ex);
                        }
                        break;
                    case 4:

                        int idTaskUpdate,
                             taskEngineerIdUpdate;
                        string? taskDescriptionUpdate,
                            taskAliasUpdate,
                            taskDeliverablesUpdate,
                            taskRemarksUpdate,
                            inputEEUpdate;
                        bool isActiveUpdate;
                        TimeSpan? requiredEffortTimeUpdate;
                        EngineerExperience? taskLevelUpdate;
                        List<BO.TaskInList?> taskInListUpdate;
                        Console.WriteLine("Enter id for reading");
                        int.TryParse(Console.ReadLine() ?? throw new Exception("enter a number please"), out idTaskUpdate);
                        BO.Task updatedTask = s_bl.Task.Read(idTaskUpdate)!;
                        Console.WriteLine(updatedTask.ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskDescriptionUpdate = Console.ReadLine()??updatedTask.Description;
                        taskAliasUpdate = Console.ReadLine()??updatedTask.Alias;
                        isActiveUpdate = updatedTask.IsActive;
                        requiredEffortTimeUpdate = updatedTask.RequiredEffortTime;
                        taskDeliverablesUpdate = Console.ReadLine()??updatedTask.Deliverables;
                        taskRemarksUpdate = Console.ReadLine()??updatedTask.Remarks;
                        inputEEUpdate = Console.ReadLine()??updatedTask.Level.ToString();
                        taskLevelUpdate = string.IsNullOrWhiteSpace(inputEEUpdate) ? updatedTask.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputEEUpdate);
                        //int.TryParse(Console.ReadLine() ?? updatedTask.Engineer.Id.ToString(), out taskEngineerIdUpdate);
                        int.TryParse(Console.ReadLine() ?? updatedTask.Milestone.Id.ToString(), out milestoneInTaskId);
                        int.TryParse(Console.ReadLine() ?? null, out taskInListId);
                        int.TryParse(Console.ReadLine() ?? updatedTask.Engineer.Id.ToString(), out taskEngineerIdUpdate);
                        //int.TryParse(Console.ReadLine() ?? updatedTask., out milestoneInTaskIdUpdate);
                        while (taskInListId != -1)
                        {
                            taskInListUpdate!.Add(new BO.TaskInList()
                            {
                                Id = taskInListId,
                                Description = s_bl.Task.Read(taskInListId)!.Description,
                                Alias = s_bl.Task.Read(taskInListId)!.Alias,
                                Status = BO.Helper.CalculateStatus(taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate)
                            });
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out taskInListId);

                        }

                        BO.Task newTaskUpdate = new BO.Task()
                        {
                            Id = idTaskUpdate,
                            Description = taskDescriptionUpdate,
                            Alias = taskAliasUpdate,
                            IsActive = isActiveUpdate,
                            CreateAt = taskCreateAtUpdate,
                            Start = taskStartUpdate,
                            ForecastDate = taskForecastDateUpdate,
                            Deadline = taskDeadlineUpdate,
                            Complete = taskCompleteUpdate,
                            Deliverables = taskDeliverablesUpdate,
                            RequiredEffortTime = requiredEffortTimeUpdate,
                            Remarks = taskRemarksUpdate,
                            Engineer = new EngineerInTask()
                            {

                                Id = taskEngineerIdUpdate,
                                Name = s_bl.Engineer.Read(taskEngineerIdUpdate)!.Name!
                            },
                            Level = taskLevelUpdate,
                            Status = BO.Helper.CalculateStatus(taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate),
                            Milestone = new MilestoneInTask()
                            {
                                Id = milestoneInTaskIdUpdate,
                                Alias = s_bl.Milestone.Read(milestoneInTaskIdUpdate)!.Alias
                            },
                            Dependencies = taskInListUpdate!
                        };
                        try { s_bl.Task.Update(newTaskUpdate); }
                        catch (Exception ex) { throw new BlFailedToUpdate($"failed to update id:{idTaskUpdate} in task", ex); }
                        ; break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        try
                        {
                            int.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a number please"), out idDelete);
                            s_bl.Task!.Delete(idDelete);
                        }
                        catch (Exception ex)
                        {
                            throw new BlFailedToDelete($"failed to delete in task", ex);
                        }
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        static void Main(string[] args)
        {
            DateTime start, end;
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
            Console.WriteLine("TASK");
            TaskMenu();
            Console.WriteLine("engineer");
            EngineerMenu();
            Console.WriteLine("Enter start date for the project");
            DateTime.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a date"), out start);
            Console.WriteLine("Enter end date for the project");
            DateTime.TryParse(Console.ReadLine() ?? throw new BlInvalidDataException("enter a date"), out end);
            Tools.EnterStartDateProject(start);
            Tools.EnterDeadLineDateProject(end);
            Console.WriteLine("Milestone");
            MilestoneMenu();
        }
    }
}


