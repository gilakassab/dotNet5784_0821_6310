﻿using DalApi;
using DalList;
using Dal;
using DO;
using Microsoft.VisualBasic.FileIO;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace DalTest
{
    internal class Program
    {
        static readonly IDal s_dal = new Dal.DalList(); //stage 2
        //private static IDependency? s_dalDependency = new DependnecyImplementation(); //stage 1
        //private static IEngineer? s_dalEngineer = new EngineerImplementation(); //stage 1
        //private static ITask? s_dalTask = new TaskImplementation(); //stage 1

        enum MainMenu { EXIT, DEPENDENCY, ENGINEER, TASK }
        enum SubMenu { EXIT, CREATE, READ, READALL, UPDATE, DELETE }

        private static void EngineerMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                int.TryParse(Console.ReadLine() ?? throw new Exception("Enter a number please"), out chooseSubMenu);

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter id, name, email, cost and a number to choose experience");
                        int idEngineer, currentNum;
                        string nameEngineer, emailEngineer;
                        EngineerExperience levelEngineer;
                        double costEngineer;
                        idEngineer = int.Parse(Console.ReadLine());
                        nameEngineer = (Console.ReadLine());
                        emailEngineer = Console.ReadLine();
                        costEngineer = double.Parse(Console.ReadLine());
                        currentNum = int.Parse(Console.ReadLine());
                        switch (currentNum)
                        {
                            case 1: levelEngineer = EngineerExperience.Expert; break;
                            case 2: levelEngineer = EngineerExperience.Junior; break;
                            case 3: levelEngineer = EngineerExperience.Rookie; break;
                            default: levelEngineer = EngineerExperience.Expert; break;
                        }
                        s_dal.Engineer.Create(new Engineer(idEngineer, nameEngineer, emailEngineer, levelEngineer, costEngineer));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Engineer!.Read(e => e.Id == id) is null)
                            Console.WriteLine("no engineer found");
                        Console.WriteLine(s_dal.Engineer!.Read(e => e.Id == id).ToString());
                        break;
                    case 3:
                        foreach (var engineer in s_dal.Engineer!.ReadAll())
                            Console.WriteLine(engineer.ToString());
                        break;
                    case 4:
                        int idEngineerUpdate, currentNumUpdate;
                        string nameEngineerUpdate, emailEngineerUpdate;
                        EngineerExperience levelEngineerUpdate;
                        double costEngineerUpdate;
                        Console.WriteLine("Enter id for reading");
                        idEngineerUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal.Engineer!.Read(e => e.Id == idEngineerUpdate).ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        nameEngineerUpdate = (Console.ReadLine());
                        emailEngineerUpdate = Console.ReadLine();
                        costEngineerUpdate = double.Parse(Console.ReadLine());
                        currentNumUpdate = int.Parse(Console.ReadLine());
                        switch (currentNumUpdate)
                        {
                            case 1: levelEngineerUpdate = EngineerExperience.Expert; break;
                            case 2: levelEngineerUpdate = EngineerExperience.Junior; break;
                            case 3: levelEngineerUpdate = EngineerExperience.Rookie; break;
                            default: levelEngineerUpdate = EngineerExperience.Expert; break;
                        }
                        s_dal.Engineer.Update(new Engineer(idEngineerUpdate, nameEngineerUpdate, emailEngineerUpdate, levelEngineerUpdate, costEngineerUpdate));
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        s_dal.Engineer!.Delete(idDelete);
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }


        private static void DependencyMenu()
        {
            int chooseSubMenu;

            do
            {
                Console.WriteLine("enum SubMenu { EXIT ,CREATE , READ, READALL ,UPDATE,DELETE }");
                chooseSubMenu = int.Parse(Console.ReadLine());

                switch (chooseSubMenu)
                {
                    case 1:
                        Console.WriteLine("Enter details for all the characteristics");
                        int dependentTask, dependsOnTask;
                        dependentTask = int.Parse(Console.ReadLine());
                        dependsOnTask = int.Parse(Console.ReadLine());
                        s_dal.Dependency.Create( new Dependency(0, dependentTask, dependsOnTask));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if (s_dal.Dependency!.Read(d => d.Id == id) is null)
                            Console.WriteLine("no dependency found");
                        Console.WriteLine(s_dal.Dependency!.Read(d => d.Id == id).ToString());
                        break;
                    case 3:
                        foreach (var dependency in s_dal.Dependency!.ReadAll())
                            Console.WriteLine(dependency.ToString());
                        break;
                    case 4:
                        int idUpdate, dependentTaskUpdate, dependsOnTaskUpdate;
                        Console.WriteLine("Enter id for reading");
                        idUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal.Dependency!.Read(d => d.Id == idUpdate).ToString());
                        Console.WriteLine("Enter details to update");
                        dependentTaskUpdate = int.Parse(Console.ReadLine());
                        dependsOnTaskUpdate = int.Parse(Console.ReadLine());
                        s_dal.Dependency!.Update(new(idUpdate, dependentTaskUpdate, dependsOnTaskUpdate));
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        s_dal.Dependency!.Delete(idDelete);
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
                        int taskEngineerId, currentTaskNum;
                        string taskDescription, taskAlias, taskDeliverables, taskRemarks;
                        bool taskMilestone;
                        DateTime taskCreateAt, taskStart, taskForecastDate, taskDeadline, taskComplete;
                        EngineerExperience taskLevel;
                        taskMilestone = bool.Parse(Console.ReadLine());
                        taskEngineerId = int.Parse(Console.ReadLine());
                        taskDescription = Console.ReadLine();
                        taskAlias = Console.ReadLine();
                        taskDeliverables = Console.ReadLine();
                        taskRemarks = Console.ReadLine();
                        taskCreateAt = DateTime.Parse(Console.ReadLine());
                        taskStart = DateTime.Parse(Console.ReadLine());
                        taskForecastDate = DateTime.Parse(Console.ReadLine());
                        taskDeadline = DateTime.Parse(Console.ReadLine());
                        taskComplete = DateTime.Parse(Console.ReadLine());
                        currentTaskNum = int.Parse(Console.ReadLine());
                        switch (currentTaskNum)
                        {
                            case 1: taskLevel = EngineerExperience.Expert; break;
                            case 2: taskLevel = EngineerExperience.Junior; break;
                            case 3: taskLevel = EngineerExperience.Rookie; break;
                            default: taskLevel = EngineerExperience.Expert; break;
                        }
                        s_dal.Task.Create(new DO.Task(0, taskDescription, taskAlias, taskMilestone, taskCreateAt, taskStart, taskForecastDate, taskDeadline, taskComplete, taskDeliverables, taskRemarks, taskEngineerId, taskLevel));
                        break;
                    case 2:
                        int id;
                        Console.WriteLine("Enter id for reading");
                        id = int.Parse(Console.ReadLine());
                        if(s_dal.Task!.Read(t => t.Id == id) is null)
                            Console.WriteLine("no task found");
                        Console.WriteLine(s_dal.Task!.Read(t => t.Id == id).ToString());
                        break;
                    case 3:
                        foreach (var task in s_dal.Task!.ReadAll())
                            Console.WriteLine(task.ToString());
                        break;
                    case 4:
                        int idTaskUpdate, currentTaskNumUpdate, taskEngineerIdUpdate;
                        string taskDescriptionUpdate, taskAliasUpdate, taskDeliverablesUpdate, taskRemarksUpdate;
                        bool taskMilestoneUpdate;
                        DateTime taskCreateAtUpdate, taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate;
                        EngineerExperience taskLevelUpdate;
                        Console.WriteLine("Enter id for reading");
                        idTaskUpdate = int.Parse(Console.ReadLine());
                        Console.WriteLine(s_dal.Task!.Read(t => t.Id == idTaskUpdate).ToString());
                        Console.WriteLine("Enter details to update");//if null to put the same details
                        taskMilestoneUpdate = bool.Parse(Console.ReadLine());
                        taskEngineerIdUpdate = int.Parse(Console.ReadLine());
                        taskDescriptionUpdate = Console.ReadLine();
                        taskAliasUpdate = Console.ReadLine();
                        taskDeliverablesUpdate = Console.ReadLine();
                        taskRemarksUpdate = Console.ReadLine();
                        taskCreateAtUpdate = DateTime.Parse(Console.ReadLine());
                        taskStartUpdate = DateTime.Parse(Console.ReadLine());
                        taskForecastDateUpdate = DateTime.Parse(Console.ReadLine());
                        taskDeadlineUpdate = DateTime.Parse(Console.ReadLine());
                        taskCompleteUpdate = DateTime.Parse(Console.ReadLine());
                        currentTaskNumUpdate = int.Parse(Console.ReadLine());
                        switch (currentTaskNumUpdate)
                        {
                            case 1: taskLevelUpdate = EngineerExperience.Expert; break;
                            case 2: taskLevelUpdate = EngineerExperience.Junior; break;
                            case 3: taskLevelUpdate = EngineerExperience.Rookie; break;
                            default: taskLevelUpdate = EngineerExperience.Expert; break;
                        }
                        s_dal.Task.Update(new(idTaskUpdate, taskDescriptionUpdate, taskAliasUpdate, taskMilestoneUpdate, taskCreateAtUpdate, taskStartUpdate, taskForecastDateUpdate, taskDeadlineUpdate, taskCompleteUpdate, taskDeliverablesUpdate, taskRemarksUpdate, taskEngineerIdUpdate, taskLevelUpdate));
                        break;
                    case 5:
                        int idDelete;
                        Console.WriteLine("Enter id for deleting");
                        idDelete = int.Parse(Console.ReadLine());
                        s_dal.Task!.Delete(idDelete);
                        break;
                    default: return;
                }
            } while (chooseSubMenu > 0 && chooseSubMenu < 6);
        }

        static void Main(string[] args)
        {
            try
            {
                //Initialization.Do(s_dalStudent, s_dalCourse, s_dalLink); //stage 1
                Initialization.Do(s_dal); //stage 2

                int chooseEntity;
                do
                {
                    Console.WriteLine("enum MainMenu { EXIT, DEPENDENCY, ENGINEER, TASK }");
                    chooseEntity = int.Parse(Console.ReadLine());

                    switch (chooseEntity)
                    {
                        case 1:
                            DependencyMenu();
                            break;
                        case 2:
                            EngineerMenu();
                            break;
                        case 3:
                            TaskMenu();
                            break;
                    }
                } while (chooseEntity > 0 && chooseEntity < 4);
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}

