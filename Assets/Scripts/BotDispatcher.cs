using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/**
 * Make Dispatcher as generic as possible. There could be several enemies. And there could be player-owned bot generators not controlled by crafter
 * BotDispatcher is the interface between Crafter or a Factory.  It knows where to start a bot, and where to send it.
 * BotDispatcher doesn't know anything about who owns it. It just dispatches bots.
 * It knows about a navpath.  A navpath is just a null with child objects that have colliders used to randomize destinations.
 * However, BotDispatcher knows nothing about those details. All it knows that it must assign a NavPath to a bot.
 * The enemy has three lanes. So the dispatcher will chose which lane. So the dispatcher DOES know about multiple lanes and it knows how to choose among them
 *
 * Interface:
 * 
 *   Transform origin - if origin is not specified, bot will start at the beginning of the NavPath
 *   NavPath[] - list of NavPaths to select from
 *   reverse - travel paths backwards
 *   
 *   DispatchBot(Bot) - Send a Bot along one of its NavPaths
 *   
 * BotDispatcher knows about:
 *   * NavPath - it maintains a list of NavPaths and will randomly send a bot along one of them
 *   * NavMeshAgents - it sets bot's speed and angular speed
 *   * Bot - it knows about bot's data
 *   * ChassisData - it knows about bot's chassis speed and turn speed
 *   
 * Note: NavPaths set to loop will never end. Bots will patrol them
 */

#pragma warning disable 649
public class BotDispatcher : MonoBehaviour
{
    [Tooltip("Dispatch bot from this point (optional).  If origin is not specified, bot will start at begining of navPath")]
    [SerializeField] Transform origin;

    [Tooltip("Bots will randomly select one of these nav paths to follow (or patrol if a looping NavPath is selected)")]
    public NavNode[] firstNodes;

    [Tooltip("Navigate NavPaths in the reverse direction")]
    [SerializeField] bool reverse = false;

    [Tooltip("Used to set which path the new bots will follow.")]
    NavNode fixedNavPath = null;

    public void DispatchBot(Bot bot)
    {
        // Set speed
        InitBot(bot);

        // select a random path
        // set starting position
        bot.transform.position = GameManager.Instance.player.homeBase.transform.position + (GameManager.Instance.player.homeBase.transform.forward * 8);
        bot.transform.rotation = GameManager.Instance.player.homeBase.transform.rotation;
        
    }

    protected void InitBot(Bot bot)
    {

        NavNode firstNode = fixedNavPath ?? firstNodes[Random.Range(0, firstNodes.Length)];
        Debug.Log($"InitNewBot({bot.name})");
        BotController controller = bot.gameObject.AddComponent<BotController>();
        controller.owner = GameManager.Instance.player;
        controller.LoadBot();
        controller.alignment = BotController.Alignment.PLAYER;
        controller.currentHealth = 1f;
        GameManager.Instance.player.bots.Add(controller);
        controller.SetRally(firstNode.GetRandomPointInNode());
        Debug.Log($"DispatchBot: I'm sending {bot.name} along NavPath {firstNode.transform.parent.name}");
    }

    public void SetFixedLane(int pathNum)
    {
        fixedNavPath = firstNodes[pathNum];
        Debug.Log($"Lane set to {firstNodes[pathNum].transform.parent.name}");
    }
}








