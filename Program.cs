using To_Dewey;
using Terminal.Gui;

/*Application.Init();

try
{
    Application.Run(new MyView());
}
finally
{
    Application.Shutdown();
}*/

public class Program{
    public static void Main(){
        Application.Init();

        using var mainWindow = new Home();

        Application.Run(mainWindow);
        Application.Shutdown();
    }
}