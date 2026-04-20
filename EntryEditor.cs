using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Terminal.Gui;
using To_Dewey;

public class EntryEditor : Window{

    private Home homeWindow;

    public EntryEditor(Entry entry = null, Action onSaved = null){
        Title = "Press Esc to Cancel";

        var dateLabel = new Label {Text = "Date:"};
        var dateField = new DateField(DateTime.Today){
            X = Pos.Right(dateLabel) + 1,
            Y = Pos.Top(dateLabel)
        };

        var monthlyLabel = new Label {Text = "Monthly Entry? ", X = Pos.Right(dateField) + 3, Y = Pos.Top(dateField)};
        var monthlyBox = new CheckBox(){
            X = Pos.Right(monthlyLabel) + 1,
            Y = Pos.Top(monthlyLabel),
            
        };
        
        ObservableCollection<string> items = ["Task", "Event", "Note"];
        var taskLabel = new Label {Text = "Type:", Y = Pos.Bottom(dateField) + 1};
        var taskType = new ComboBox{
            X = Pos.Right(taskLabel) + 1,
            Y = Pos.Top(taskLabel),            
            Width = Dim.Percent (42),
            Height = 4,
            HideDropdownListOnClick = true
        };
        taskType.SetSource(items);

        var bodyLabel = new Label {Text = "Body:", Y = Pos.Bottom(taskLabel) + 3};
        var bodyText = new TextView{
            X = Pos.Right(bodyLabel) + 1,
            Y = Pos.Top(bodyLabel),
            Width = Dim.Fill(40),
            Height = 10,
            WordWrap = true,
        };

        var addBtn = new Button{
            Text = "Add",
            Y = Pos.Bottom(bodyText) + 1,
            X = Pos.Center(),
            IsDefault = true
        };

        //Runs when the Edit button is clicked on home screen.
        if(entry != null){
            monthlyBox.CheckedState = entry.isMonthly ? CheckState.Checked : CheckState.UnChecked;

            dateField.Date = entry.date;
            taskType.SelectedItem = entry.taskTypes;
            bodyText.Text = entry.body;
        }

        //Runs when the Add button is clicked in the editor.
        addBtn.Accepting += (s,e) => {
            bool isMonthlyChecked = monthlyBox.CheckedState == CheckState.Checked;
            string body = bodyText.Text?.ToString() ?? "";

            if(entry == null){
                Entry note = new Entry(){
                    date = dateField.Date,
                    isMonthly = isMonthlyChecked,
                    taskTypes = taskType.SelectedItem,
                    body = body
                };
            
                Home.notes.Add(note);
            }

            else{
                entry.date = dateField.Date;
                entry.isMonthly = isMonthlyChecked;
                entry.taskTypes = taskType.SelectedItem;
                entry.body = body;
            }
            
            onSaved?.Invoke();

            MessageBox.Query("Success", "Note Saved", "Ok");
            Application.RequestStop();
        };

        Add(dateLabel, dateField, monthlyLabel, monthlyBox, taskLabel, taskType, bodyLabel, bodyText, addBtn);
    }
}