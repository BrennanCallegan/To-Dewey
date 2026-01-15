using System;
using Terminal.Gui;

public class EntryEditor : Window{

    public EntryEditor(){
        Title = "Press Esc to Cancel";
        
        var bodyLabel = new Label {Text = "Body:"};
        var body = new TextField{
            X = Pos.Right(bodyLabel) + 1,
            Width = Dim.Fill()
        };

        var addBtn = new Button{
            Text = "_Add",
            Y = Pos.Bottom(bodyLabel) + 1,
            X = Pos.Center(),
            IsDefault = true
        };

        addBtn.Accepting += (s,e) => {

        };
    }
}