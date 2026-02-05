using To_Dewey;
using Terminal.Gui;
using static System.Console;

public class Entry{

    public DateTime date = DateTime.Now;

    public int taskTypes {get; set;}

    public String body{get; set;}

    public override String ToString(){
        if(taskTypes == 0){return "• " + body;}
        if(taskTypes == 1){return "○ " + body;}
        else{return "- " + body;}

        
    }
}