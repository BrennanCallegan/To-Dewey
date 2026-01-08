using static System.Console;

public class Entry{

    private DateTime date = DateTime.Now;

    private String body{get; set;}

    public override String ToString(){
        return date + "-" + body;
    }
}