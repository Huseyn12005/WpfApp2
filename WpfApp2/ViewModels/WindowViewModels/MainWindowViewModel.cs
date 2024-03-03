using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfApp2.Commands;
using WpfApp2.Data;
using WpfApp2.Services;
using WpfApp2.Views.Windows;

namespace WpfApp2.ViewModels.WindowViewModels
{
    

    public class MainWindowViewModel:NotificationService
    {
        private string _Api { get; set; }
        public string Api
        {
            get { return _Api; }
            set { _Api = value;OnPropertyChanged(); }
        }
        public ObservableCollection<Comment> comments { get; set; }
        public ICommand? GetApiCommand { get; set; }
        public ICommand? RemoveCommand { get; set; }

        private Comment _Comment;

        public Comment Comment
        {
            get { return _Comment; }
            set { _Comment = value; OnPropertyChanged(); }
        }

        public Window1 mainWindow { get; set; }

        public MainWindowViewModel(Window1 _mainWindow)
        {

            RemoveCommand = new RelayCommand(
                async remove =>
                {
                    await Remove();
                },
                a => true);
            mainWindow = _mainWindow;
            comments = new ObservableCollection<Comment>();

            GetApiCommand = new RelayCommand(GetAllDataFromJsonApi);
        }
        private async Task<bool> Check(object? parameter)
        {

            if (Api != null)
            {
                string fileName = "comments.json";

                string directoryPath = @"../../../Database";

                string filePath = System.IO.Path.Combine(directoryPath, fileName);

                if (File.Exists(filePath))
                {
                    return false;
                }
                else
                {
                    return true;
                }
               
            }
            return false;
        }

        private async void GetAllDataFromJsonApi(object? param)
        {
            string apiUrl = "https://jsonplaceholder.typicode.com/comments";
            string FileName = "comments.json";

            string directoryPath = @"../../../Database";

            string filePath = System.IO.Path.Combine(directoryPath, FileName);

            if (!File.Exists(filePath))
            {
                try
                {
                    using var httpClient = new HttpClient();

                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonString = await response.Content.ReadAsStringAsync();

                        File.WriteAllText(filePath,jsonString);
                        var commentsFromFile = JsonConvert.DeserializeObject<ObservableCollection<Comment>>(filePath);

                        string serializedJson = System.Text.Json.JsonSerializer.Serialize(filePath);

                        File.WriteAllText(FileName, serializedJson);
                        await Task.Delay(5000);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }

            try
            {
                if (File.Exists(filePath))
                {
                    string json = File.ReadAllText(filePath);
                    var commentsFromFile = JsonConvert.DeserializeObject<ObservableCollection<Comment>>(json);
                    foreach (var comment in commentsFromFile!)
                    {
                        comments!.Add(comment);
                        await Task.Delay(10);
                    }
                }
                else
                    MessageBox.Show($"File not found: {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading cars from JSON file: {ex.Message}");
            }


        }




        private async Task Remove()
        {
            if (comments == null)
                return;

            string FileName = "comments.json";

            string directoryPath = @"../../../Database";

            string filePath = System.IO.Path.Combine(directoryPath, FileName);

            try
            {
                Comment? selectedItem = mainWindow.List.SelectedItem as Comment;

                if (selectedItem != null )
                {
                    comments.Remove(selectedItem);
                    string json = JsonConvert.SerializeObject(comments, Formatting.Indented);
                    await File.WriteAllTextAsync(filePath!, json);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}");
            }
        }
    }




    }
