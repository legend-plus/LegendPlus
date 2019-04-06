using Discord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class discord_integration : MonoBehaviour
{
    public Discord.Discord discord;
    public Discord.UserManager userManager;
    public Discord.ActivityManager activityManager;
    public int loopCounter = 0;
    public int loopUpdateRate = 60 * 5;
    // Use this for initialization
    void Start()
    {
        const long CLIENT_ID = 499069912048992271;
        discord = new Discord.Discord(CLIENT_ID, (System.UInt64)Discord.CreateFlags.Default);
        userManager = discord.GetUserManager();
        activityManager = discord.GetActivityManager();
        UpdateActivity();
    }

    // Update is called once per frame
    void Update()
    {
        discord.RunCallbacks();
        //UpdateActivity(discord);
        loopCounter++;
        if ((loopCounter % loopUpdateRate) == 0)
        {
            UpdateActivity();
        }
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Thank you for playing Wing Commander.");
        discord.Dispose();
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
                    CurrentSize = Random.Range(1, 8),
                    MaxSize = Random.Range(8, 100),
                },
            },
            Instance = false,
        };

        activityManager.UpdateActivity(activity, result =>
        {
            Debug.Log("Update Activity {0}" + result.ToString());

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
