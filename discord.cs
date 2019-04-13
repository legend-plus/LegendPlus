using Godot;
using System;
using Discord;

public class discord : Node2D
{
    public Discord.Discord discord_instance;
    public Discord.UserManager userManager;
    public Discord.ActivityManager activityManager;
    public Discord.ApplicationManager appManager;
    public int loopCounter = 0;
    public int loopUpdateRate = 60 * 5;
    public string accessToken = "";
    public Random rng;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        rng = new Random();
        const long CLIENT_ID = 499069912048992271;
        discord_instance = new Discord.Discord(CLIENT_ID, (System.UInt64)Discord.CreateFlags.Default);
        userManager = discord_instance.GetUserManager();
        activityManager = discord_instance.GetActivityManager();
        appManager = discord_instance.GetApplicationManager();
        
        appManager.GetOAuth2Token((Discord.Result result, ref Discord.OAuth2Token oauth2Token) =>
        {
            GD.Print(result);
            GD.Print('"' + oauth2Token.AccessToken + '"');
            accessToken = oauth2Token.AccessToken;
        });
        UpdateActivity();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		discord_instance.RunCallbacks();
        //UpdateActivity(discord);
        loopCounter++;
        if ((loopCounter % loopUpdateRate) == 0)
        {
            UpdateActivity();
        }
	}
	
	private void _exit_tree()
    {
        GD.Print("Thank you for playing Wing Commander.");
        discord_instance.Dispose();
    }
	
	public long UnixTimeNow()
    {
        var timeSpan = (System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0));
        return (long)timeSpan.TotalSeconds;
    }

    public void UpdateActivity()
    {
        long now = UnixTimeNow();
        var activity = new Discord.Activity
        {
            State = "Playing tester",
            Details = "Playing Details",
            Timestamps =
            {
                Start = now
            },
            Assets =
            {
                LargeImage = "better",
                LargeText = "Large Image Text",
                SmallImage = "zahra",
                SmallText = "Small Image Text",
            },
            Party = {
               Id = "ae488379-351d-4a4f-ad32-2b9b01c91657",
               Size = {
                    CurrentSize = rng.Next(1,8),
                    MaxSize = rng.Next(8, 100),
                },
            },
            Instance = false,
        };

        activityManager.UpdateActivity(activity, result =>
        {
            GD.Print("Update Activity {0}" + result.ToString());

            // Send an invite to another user for this activity.
            // Receiver should see an invite in their DM.
            // Use a relationship user's ID for this.
            // activityManager
            //   .SendInvite(
            //       364843917537050624,
            //       Discord.ActivityActionType.Join,
            //       "",
            //       inviteResult =>
            //       {
            //           Console.WriteLine("Invite {0}", inviteResult);
            //       }
            //   );
        });
    }
}
