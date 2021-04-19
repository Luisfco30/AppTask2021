using AppTask2021.Models;
using AppTask2021.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppTask2021.ViewModels
{
    public class TaskListViewModel : BaseViewModel
    {
        private static TaskListViewModel instance;

        Command newTaskCommand;
        public Command NewTaskCommand => newTaskCommand ?? (newTaskCommand = new Command(NewTaskAction));

        List<TaskModel> tasks;
        public List<TaskModel> Tasks
        {
            get => tasks;
            set => SetProperty(ref tasks, value);
        }

        TaskModel taskSelected;
        public TaskModel TaskSelected
        {
            get => taskSelected;
            set 
            {
                if (SetProperty(ref taskSelected, value))
                {
                    SelectTaskAction();
                }
            }
        }

        public TaskListViewModel()
        {
            instance = this;
            LoadTasks();
        }

        public static TaskListViewModel GetInstance()
        {
            return instance;
        }

        public async void LoadTasks()
        {
            Tasks = await App.SQLiteDatabase.GetAllTasksAsync();
        }

        private void NewTaskAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TaskDetailPage());
        }

        private void SelectTaskAction()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TaskDetailPage(taskSelected));
        }

    }
}
