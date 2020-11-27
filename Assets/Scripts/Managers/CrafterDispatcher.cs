using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Sends newly bots on their way
// Note: Is this a singleton?
// Should this class know about Crafter?  It registers with it. So it cant be used with the enemy. That's bad. It's tightly coupled to Crafter
// This guy should maybe be a singleton
// Player could have several bases, in which case, each base would have a dispatcher
// Each dispatcher could even own a path
public class CrafterDispatcher : BotDispatcher
{
    protected void Start()
    {
        // listen for new bots from the Crafter
        Crafter.Instance.onNewBot += DispatchBot;
    }

    private void OnDisable()
    {
        Crafter.Instance.onNewBot -= DispatchBot;
    }
}