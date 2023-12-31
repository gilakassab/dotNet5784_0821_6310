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

        }
        public static void EngineerMenu()
        {
            int chooseSubMenu;
            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new Exception("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter id, name,isactive, email, level, cost and role");
                        int idEngineer,
                            idTask ;
                        string nameEngineer,
                               emailEngineer,
                               inputEE,
                               inputR; ;
                        DO.EngineerExperience levelEngineer;
                        DO.Roles role;
                        bool isActive;
                        double costEngineer;
                        idEngineer = int.Parse(Console.ReadLine());
                        nameEngineer = (Console.ReadLine());
                        isActive = Console.ReadLine() == "false" ? false : true;
                        emailEngineer = Console.ReadLine();
                        inputEE = Console.ReadLine();
                        inputR = Console.ReadLine();
                        levelEngineer = (DO.EngineerExperience)Enum.Parse(typeof(DO.EngineerExperience), inputEE);
                        costEngineer = double.Parse(Console.ReadLine());
                        role = (DO.Roles)Enum.Parse(typeof(DO.Roles), inputR);
                        idTask = int.Parse(Console.ReadLine());

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
                                Alias = s_bl.Task.Read(idTask).Alias
                            }
                        };
                        try
                        {
                            s_bl.Engineer.Create(newEng);
                        }
                        catch(Exception ex)
                        {
                            throw new BlFailedToCreate("Failed to build engineer ", ex);
                        }
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        try {
                            if (s_bl.Engineer!.Read(id) is null)
                                Console.WriteLine("no engineer found");
                            else
                            {
                                Console.WriteLine(s_bl.Engineer!.Read(id).ToString());
                            }
                            }
                        catch(Exception ex){
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
                        string nameEngineerUpdate,
                            emailEngineerUpdate, 
                            inputUpdateEE,
                            inputUpdateR;
                        EngineerExperience levelEngineerUpdate;
                        double costEngineerUpdate;
                        bool isActiveUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine());
                        try
                        {
                            Engineer updatedEngineer = s_bl.Engineer.Read(idEngineerUpdate);
                            Console.WriteLine(updatedEngineer.ToString());
                            Console.WriteLine("Enter name, isactive,level,cost,role and id of task to update");//if null to put the same details
                            nameEngineerUpdate = Console.ReadLine() ?? updatedEngineer?.Name;
                            isActiveUpdate = Console.ReadLine() == "false" ? false : true;
                            emailEngineerUpdate = Console.ReadLine() ?? updatedEngineer?.Email;
                            inputUpdateEE = Console.ReadLine();
                            inputUpdateR = Console.ReadLine();
                            levelEngineerUpdate = string.IsNullOrWhiteSpace(inputUpdateEE) ? updatedEngineer.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputUpdate);
                            costEngineerUpdate = double.Parse(Console.ReadLine());
                            role = (DO.Roles)Enum.Parse(typeof(DO.Roles), inputUpdateR);
                            idTaskUpdate = int.Parse(Console.ReadLine());
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
                                    Alias = s_bl.Task.Read(idTaskUpdate).Alias
                                }
                            };
                            try
                            {
                                s_bl.Engineer.Update(newEngUpdate);
                            }
                            catch (Exception ex)
                            {
                                throw new BlFailedToCreate($"failed to update engineer id={idEngineerUpdate}");
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
                        idDelete = int.Parse(Console.ReadLine());
                        try {
                            s_bl.Engineer!.Delete(idDelete);
                        }
                        catch(Exception ex)
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
                chooseSubMenu = int.Parse(Console.ReadLine());
                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter  description, alias,deriverables, remarks,milestone, dates and task's level");
                        int taskId,
                            taskEngineerId,
                            milestoneInTaskId,
                            engineerInTaskId,
                            taskInListId;
                        string taskDescription,
                               taskAlias,
                               taskDeliverables,
                               taskRemarks,
                               inputEE;
                        bool taskMilestone,
                             isActive;
                        TimeSpan requiredEffortTime;
                        DateTime taskCreateAt, 
                                 taskStart, 
                                 taskForecastDate, 
                                 taskDeadline, 
                                 taskComplete;
                        EngineerExperience taskLevel;
                        Status statusTask;
                        List<BO.TaskInList?>taskInList=null;
                        taskId = int.Parse(Console.ReadLine());
                        taskDescription = Console.ReadLine();
                        taskAlias = Console.ReadLine();
                        isActive = Console.ReadLine() == "false" ? false : true;
                        taskCreateAt = DateTime.Parse(Console.ReadLine());
                        taskStart = DateTime.Parse(Console.ReadLine());
                        taskForecastDate = DateTime.Parse(Console.ReadLine());
                        taskDeadline = DateTime.Parse(Console.ReadLine());
                        taskComplete = DateTime.Parse(Console.ReadLine());
                        taskDeliverables = Console.ReadLine();
                        requiredEffortTime = TimeSpan.Zero;
                        taskRemarks = Console.ReadLine();
                        engineerInTaskId = int.Parse(Console.ReadLine());
                        inputEE = Console.ReadLine();
                        taskLevel = (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputEE);
                        taskEngineerId = int.Parse(Console.ReadLine());
                        milestoneInTaskId = int.Parse(Console.ReadLine());
                        taskInListId = int.Parse(Console.ReadLine());
                        while (taskInListId != -1)
                        {
                            taskInList.Add(new BO.TaskInList()
                            {
                                Id = taskInListId,
                                Description = s_bl.Task.Read(taskInListId).Description,
                                Alias = s_bl.Task.Read(taskInListId).Alias,
                                Status = BO.Helper.CalculateStatus(taskStart, taskForecastDate, taskDeadline, taskComplete)
                            });
                            taskInListId = int.Parse(Console.ReadLine());
                        }
                        BO.Task newTask = new BO.Task()
                        {
                            Id = taskId,
                            Description = taskDescription,
                            Alias = taskAlias,
                            IsActive = isActive,
                            CreateAt = taskCreateAt,
                            Start = taskStart,
                            ForecastDate = taskForecastDate,
                            Deadline = taskDeadline,
                            Complete = taskComplete,
                            Deliverables = taskDeliverables,
                            RequiredEffortTime = requiredEffortTime,
                            Remarks = taskRemarks,
                            Engineer = new EngineerInTask()
                            {
                                Id = engineerInTaskId,
                                Name = s_bl.Engineer.Read(engineerInTaskId).Name
                            },
                            Level = taskLevel,
                            Status = BO.Helper.CalculateStatus(taskStart, taskForecastDate, taskDeadline, taskComplete),
                            Milestone = new MilestoneInTask()
                            {
                                Id = milestoneInTaskId,
                                Alias = s_bl.Milestone.Read(milestoneInTaskId).Alias
                            },
                            Dependencies = taskInList
                        };
                        try { s_bl.Task.Create(newTask); }
                        catch(Exception ex) { throw new BlFailedToCreate("failed to create task", ex); }
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        try
                        {
                            if (s_bl.Task!.Read(id) is null)
                                Console.WriteLine("no task found");
                            else
                                Console.WriteLine(s_bl.Task!.Read(id).ToString());
                        }
                        catch(Exception ex)
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
                             milestoneInTaskIdUpdate,
                             engineerInTaskIdUpdate,
                            taskInListIdUpdate;
                        string taskDescriptionUpdate, 
                            taskAliasUpdate, 
                            taskDeliverablesUpdate, 
                            taskRemarksUpdate, 
                            inputEEUpdate;
                        bool taskMilestoneUpdate, 
                            isActiveUpdate;
                        DateTime taskCreateAtUpdate, 
                                 taskStartUpdate, 
                                 taskForecastDateUpdate, 
                                 taskDeadlineUpdate, 
                                 taskCompleteUpdate;
                        TimeSpan requiredEffortTimeUpdate;
                        EngineerExperience taskLevelUpdate;
                        Status statusTaskUpdate;
                        List<BO.TaskInList?> taskInListUpdate = null;
                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine());
                        BO.Task updatedTask = s_bl.Task.Read(idTaskUpdate);
                        Console.WriteLine(updatedTask.ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskDescriptionUpdate = Console.ReadLine();
                        taskAliasUpdate = Console.ReadLine();
                        isActiveUpdate = Console.ReadLine() == "false" ? false : true;
                        taskCreateAtUpdate = DateTime.Parse(Console.ReadLine());
                        taskStartUpdate = DateTime.Parse(Console.ReadLine());
                        taskForecastDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskDeadlineUpdate = DateTime.Parse(Console.ReadLine());
                        taskCompleteUpdate = DateTime.Parse(Console.ReadLine());
                        requiredEffortTimeUpdate = TimeSpan.Zero;
                        taskDeliverablesUpdate = Console.ReadLine();
                        taskRemarksUpdate = Console.ReadLine();
                        inputEEUpdate = Console.ReadLine();
                        taskLevelUpdate = string.IsNullOrWhiteSpace(inputEEUpdate) ? updatedTask.Level : (EngineerExperience)Enum.Parse(typeof(EngineerExperience), inputUpdate);
                        taskEngineerId = int.Parse(Console.ReadLine());
                        milestoneInTaskId = int.Parse(Console.ReadLine());
                        taskInListId = int.Parse(Console.ReadLine());
                        engineerInTaskIdUpdate = int.Parse(Console.ReadLine());
                        milestoneInTaskIdUpdate = int.Parse(Console.ReadLine());
                        while (taskInListId != -1)
                        {
                            taskInListUpdate.Add(new BO.TaskInList()
                            {
                                Id = taskInListId,
                                Description = s_bl.Task.Read(taskInListId).Description,
                                Alias = s_bl.Task.Read(taskInListId).Alias,
                                Status = BO.Helper.CalculateStatus(taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate)
                            });
                            taskInListId = int.Parse(Console.ReadLine());
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
              
                                Id = engineerInTaskIdUpdate,
                                Name = s_bl.Engineer.Read(engineerInTaskIdUpdate).Name
                            },
                            Level = taskLevelUpdate,
                            Status = BO.Helper.CalculateStatus(taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate),
                            Milestone = new MilestoneInTask()
                            {
                                Id = milestoneInTaskIdUpdate,
                                Alias = s_bl.Milestone.Read(milestoneInTaskIdUpdate).Alias
                            },
                            Dependencies = taskInListUpdate
                        };
                        try { s_bl.Task.Update(newTaskUpdate); }
                        catch(Exception ex) { throw new BlFailedToUpdate($"failed to update id:{idTaskUpdate} in task", ex); }
                        ; break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        try
                        {
                            idDelete = int.Parse(Console.ReadLine());
                            s_bl.Task!.Delete(idDelete);
                        }
                        catch(Exception ex)
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
            Console.Write("Would you like to create Initial data? (Y/N)");
            string? ans = Console.ReadLine() ?? throw new FormatException("Wrong input");
            if (ans == "Y")
                DalTest.Initialization.Do();
            try { int chooseEntity;
            do
            {
                Console.WriteLine("enum MainMenu {  EXIT, MILESTONE, ENGINEER, TASK }");
                chooseEntity = int.Parse(Console.ReadLine());
                switch (chooseEntity)
                {
                    case 1:
                        MilestoneMenu();
                        break;
                    case 2:
                        EngineerMenu();
                        break;
                    case 3:
                        TaskMenu();
                        break;
                }
            } while (chooseEntity >= 0 && chooseEntity < 4);}
            
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        } } }
    




