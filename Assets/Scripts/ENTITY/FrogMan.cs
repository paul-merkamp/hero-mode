using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogMan : MonoBehaviour
{
    private PlayerController player;
    private DialogController dialog;
    private GameEventController gameEventController;

    public bool frogManQuestCompleted = false;

    private List<string> frogFacts = new List<string>{
        "\"These <color=#55d535>frogs</color>' homes were destroyed by a monster.\"",
        "\"Did you know that <color=#55d535>frogs</color> breathe through their skin?\"",
        "\"I love <color=#55d535>frogs</color>... they are just so cute!\"",
        "\"Some <color=#55d535>frogs</color> can jump over 20 times their own body length.\"",
        "\"Did you know? Teaching children anatomy via dissecting <color=#55d535>frogs</color> is evil.\"",
        "\"Did you know? I love <color=#55d535>frogs</color>.\"",
        "\"Every minute, one trillion <color=#55d535>frogs</color> are killed for their legs.\"",
        "\"I met a <color=#55d535>frog</color> named <color=#55d535>Greggo</color>. He is my best friend.\"",
        "\"Ribbit!\""
    };

    public int currentFactIndex = 0;

    public int requiredTokens = 10;

    public void Awake()
    {
        player = FindAnyObjectByType<PlayerController>();
        dialog = FindAnyObjectByType<DialogController>();
        gameEventController = FindAnyObjectByType<GameEventController>();

        if (PlayerData.frogManQuestCompleted)
        {
            gameObject.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!dialog.isDialogActive)
        {
            if (player.frogTokens < requiredTokens)
            {
                dialog.TriggerDialog(
                    new List<string>
                    {
                        frogFacts[currentFactIndex],
                        "\"Can you bring me <color=#55d535>" + requiredTokens + " frog tokens</color>?\""
                    }
                );

                currentFactIndex = (currentFactIndex + 1) % frogFacts.Count;
            }
            else 
            {
                frogManQuestCompleted = true;
                
                gameEventController.TriggerEvent_11();
            }
        }
    }

    [System.Obsolete]
    public void SpawnHeartContainer()
    {
        GameObject.Find("ENV/ENV_Objects/NotDistanceLoaded/FrogHeartContainer").SetActiveRecursively(true);
    }
}
