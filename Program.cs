using To_Dewey;
using Terminal.Gui;

public class Program{
    public static void Main(){
        Application.Init();

        using var mainWindow = new Home();

        Application.Run(mainWindow);
        Application.Shutdown();
    }
}