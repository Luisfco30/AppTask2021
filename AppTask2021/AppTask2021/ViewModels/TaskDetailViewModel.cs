using AppTask2021.Models;
using AppTask2021.Services;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace AppTask2021.ViewModels
{

    public class TaskDetailViewModel : BaseViewModel
    {
        Command takePictureCommand;
        public Command TakePictureCommand => takePictureCommand ?? (takePictureCommand = new Command(TakePictureAction));

        Command selectPictureCommand;
        public Command SelectPictureCommand => selectPictureCommand ?? (selectPictureCommand = new Command(SelectPictureAction));

        Command saveCommand;
        public Command SaveCommand => saveCommand ?? (saveCommand = new Command(SaveAction));

        Command deleteCommand;
        public Command DeleteCommand => deleteCommand ?? (deleteCommand = new Command(DeleteAction));

        Command cancelCommand;
        public Command CancelCommand => cancelCommand ?? (cancelCommand = new Command(CancelAction));

        TaskModel taskSelected;
        public TaskModel TaskSelected
        {
            get => taskSelected;
            set => SetProperty(ref taskSelected, value);
        }

        string imageBase64;
        public string ImageBase64
        {
            get => imageBase64;
            set => SetProperty(ref imageBase64, value);
        }

        public TaskDetailViewModel()
        {
            TaskSelected = new TaskModel();
        }

        public TaskDetailViewModel(TaskModel taskSelected) {
            TaskSelected = taskSelected;
            ImageBase64 = taskSelected.ImageBase64;
        }

        private async void TakePictureAction()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTask", "No existe cámara disponible en el dispositivo", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "AppTask",
                    Name = "TaskPicture.jpg"
                });

                if (file == null)
                    return;

                TaskSelected.ImageBase64 = ImageBase64 = await new ImageService().ConvertImageFilePathToBase64(file.Path);

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppTask", $"Se generó un error al tomar la fotografía ({ex.Message})", "Ok");
            }
        }

        private async void SelectPictureAction()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("AppTask", "No es posible seleccionar fotografías en el dispositivo", "Ok");
                    return;
                }

                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Medium
                });

                if (file == null)
                    return;

                TaskSelected.ImageBase64 = ImageBase64 = await new ImageService().ConvertImageFilePathToBase64(file.Path);
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("AppTask", $"Se generó un error al tomar la fotografía ({ex.Message})", "Ok");
            }
        }

        private async void SaveAction()
        {
            await App.SQLiteDatabase.SaveTaskAsync(taskSelected);
            TaskListViewModel.GetInstance().LoadTasks();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void DeleteAction()
        {
            await App.SQLiteDatabase.DeleteTaskAsync (taskSelected);
            TaskListViewModel.GetInstance().LoadTasks();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async void CancelAction()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

    }
}
