using To_Dewey;
using Terminal.Gui;
using static System.Console;

public class Entry{

    public bool isMonthly {get; set;}
    
    public DateTime date {get; set;}

    public int taskTypes {get; set;}

    public String body{get; set;}

    public override String ToString(){
        if(taskTypes == 0){return "• " + body;}
        if(taskTypes == 1){return "○ " + body;}
        else{return "- " + body;}

        
    }
}