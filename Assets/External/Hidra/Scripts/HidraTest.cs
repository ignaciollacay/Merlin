using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidraTest : EnemyStats
{
    /// SCRIPT SUMMARY
    /// 
    /// Spawn a monster with a random amount of heads (3-12)
    /// And with a random numbered head with different settings from the others
    /// 
    /// When the player touches over a head (eventually shoots an arrow)
    /// If the head is the random numbered head or a multiple of it within the total amount of heads
    /// then the head will die
    /// Else,
    /// the head will die and then regenerate
    /// Only when all the head-multiples are killed, the monster will die

    //-- 1. CHOOSE RANDOM MULTIPLE
    //-- 2. CHOOSE RANDOM AMOUNT OF HEADS
            //SPAWN WITH ABOVE SETTINGS?
    //-- 3. ASSIGN SETTINGS TO "FIRST MULTIPLE" HEAD (FirstMultiple Animation)
    //-- 4. ASSIGN SETTINGS TO CORRECT HEADS (No-Respawn; CorrectHead Animation Help Guide -- maybe when attacking?)
    //-- 5. ASSIGN SETTINGS TO INCORRECT HEADS (Respawn when killed)
    //-- 6. ASSIGN COMMON SETTINGS: Death, Attack,
    //-- 7. HEAD DEATH CONDITIONS
    //-- 8. HIDRA DEATH CONDITIONS

    /// SPAWNING
    /// Once the random numbers are defined

    int firstMultipleNumber; //numero multiplo cabeza
    GameObject firstMultipleHead;
    int headAmount;

    List<int> multiples;
    int multiple = 1;
    public int currentMultiplier = 1;

    int headMultipleAmount = 0;

    [Header("Default Settings")]
    //[SerializeField] GameObject hidraPrefab;
    [SerializeField] List<GameObject> headsCTR; //heads sorted from center to edges
    [SerializeField] List<GameObject> headsLTR; //heads sorted from left to right
    [SerializeField] int maxHeadAmount = 12;


    [Header("Attack Properties")]
    [SerializeField] int hidraAttackDelay = 10; //faltaria un max y min para randomizar
    [SerializeField] int hidraAttackProbability = 50;
    [SerializeField] int hidraAttackDamage = 10;
    [SerializeField] GameObject hidraAttackVFX;
    [SerializeField] Transform throwPoint;

    //[SerializeField] PlayerStats _PlayerStats;


    [Header("Sound Events")]
    public string hidraDeath = "FMOD Event Path";


    public override void Awake()
    {
        base.Awake();

        ChooseFirstMultiple();
        ChooseHeadAmount();
        SetHeadAmount();
        ///<TBD>
        ///Spawn the object. Instanciate Hidra Prefab with current settings.
        ///But Script references should be linked not on Heirarchy/Scene but the Inspector/Script
        ///</TBD>
        MultipleCalculator();
        SetFirstMultiple();
        SetHeadMultiples();

        //HeadMark();
    }

    public void Start()
    {
        StartCoroutine(HidraAttackCoroutine());
    }

    // 1. CHOOSE RANDOM MULTIPLE
    void ChooseFirstMultiple()
    {
        // NUMBER...
        firstMultipleNumber = Random.Range(2, 5); //definir aleatoriamente el primer multiplo
        //Debug.Log("Selected first multiple is " + firstMultipleNumber);
    }

        // 2. CHOOSE RANDOM AMOUNT OF HEADS
    void ChooseHeadAmount()
    {
        int minHeadAmount = firstMultipleNumber * 2; //que hayan al menos dos cabezas a disparar por hidra
        headAmount = Random.Range(minHeadAmount, maxHeadAmount);
        //Debug.Log("Hidra head amount is " + headAmount);
    }

    //TBD - Works, but shoudn't need to destroy if never spawned.
    void SetHeadAmount()
    {
        //Delete heads in list/array above the array value.
        
        // option 2:  works but leaves list slot
        //DESTROY HEADS ABOVE HEAD AMOUNT
        int index = 0;
        foreach (GameObject hidraHead in headsCTR)
        {
            index++;
            if (index > headAmount)
            {
                hidraHead.SetActive(false); //better destroying, but cant remove the heads from LTR head array
                //Destroy(hidraHead);
            }
        }
        //option 1: Works. Needs Spawn to work properly
        //HEAD AMOUNT == LIST RANGE
        headsCTR.RemoveRange(headAmount, (maxHeadAmount - headAmount));
    }

    void SetFirstMultiple()
    {
        //removes the heads that were disabled because they were above the current hidra head amount, so that the array values align with different sort order (left to right instead of centered)
        for (var i = headsLTR.Count - 1; i > -1; i--)
        {
            if (headsLTR[i].activeInHierarchy == false)
                headsLTR.RemoveAt(i); 
        }
        // hago algo similar en HeadMark, pero es otro index y tengo que ejecutarlo varias veces.
        // podria pasarle un parametro a la funcion, empezando con firstMultipleNumber y siguiendo con currentMultiplier
        firstMultipleHead = headsLTR[firstMultipleNumber - 1];
        //Debug.Log("Hidra first marked head is " + firstMultipleHead.name, firstMultipleHead);
        firstMultipleHead.GetComponent<HidraHeadTest>().marked = true;

        //better way, not yet implemented. TBD.
        //HeadMark();
    }

    void MultipleCalculator()
    {

        int multiplier = 1;
        //.int multiple = 1; // moved variable to class

        multiples = new List<int>(); // moved List to Class

        bool continueCalculating = true;
        do
        {
            multiple = firstMultipleNumber * multiplier;
            if (multiple <= headAmount)
            {
                multiples.Add(multiple);
            }
            if (multiple > headAmount)
            {
                continueCalculating = false;
            }
            multiplier++;
        } while (continueCalculating);

        if (!continueCalculating)
        {
            foreach (int multiple in multiples)
            {
                //Debug.Log("Multiples: " + multiple);
            }
        }
    }

    void SetHeadMultiples()
    {
        // ASSIGN MULTIPLE BOOL-STATE FOR MULTIPLE VALUES CALCULATED IN MULTIPLECALCULATOR FX
        
        foreach (int _multiple in multiples)
        {
            //Debug.Log("Multiple for this Hidra: " + _multiple + " : " + headsLTR[_multiple - 1].name, headsLTR[_multiple - 1]);
            headsLTR[_multiple - 1].GetComponent<HidraHeadTest>().multiple = true;

            headMultipleAmount++; //add to counter of amount of multiple heads for win condition check
        }
    }

    /* better way, not yet implemented. TBD.
    public void SetHeadMark(int headNumber)
    {
        headNumber = headNumber - 1;

        headsLTR[headNumber].GetComponent<HidraHeadTest>().marked = true;
        Debug.Log("Marked head is " + headsLTR[headNumber].name, headsLTR[headNumber]);
    }
    */
    public void HeadMark()
    {
        ///TBD
        /// Solo asignar la marca si la cabeza no esta muerta. Si esta muerta volver a ejectuar la funcion.

        currentMultiplier++; //definir proximo multiplicador para definir el prox multiplo
        int newHeadMarked = (firstMultipleNumber * currentMultiplier) - 1; //definir el numero de cabeza del proximo multiplo
        Debug.Log("current multiplier is " + currentMultiplier);
        if (newHeadMarked <= headAmount)
        {
            HidraHeadTest _HidraHeadTest = headsLTR[newHeadMarked].GetComponent<HidraHeadTest>();
            if (_HidraHeadTest.deadHead == false)
            {
                _HidraHeadTest.marked = true;
                Debug.Log("HeadMark current is " + headsLTR[newHeadMarked] + ". Mark state is " + headsLTR[newHeadMarked].GetComponent<HidraHeadTest>().marked);
            }
            else if (_HidraHeadTest.defeated)
            {
                return;
            }
            else
            {
                HeadMark();
            }
        }
    }

    //contrastar cantidad multiplos con cantidad cabezas muertas
    //called when a head is killed
    public void AllHeadsKilled()
    {
        int deadHeads = 0;

        foreach (GameObject hidraHead in headsLTR)
        {
            HidraHeadTest _hidraHead = hidraHead.GetComponent<HidraHeadTest>();

            if ((_hidraHead.multiple) && (_hidraHead.deadHead))
            {
                deadHeads++;
            }
        }

        if (deadHeads == headMultipleAmount)
        {
            //hidra monster has been defeated.
            //death animation for the non-multiple heads and body
            foreach (GameObject hidraHead in headsLTR)
            {
                //allow non-multiple heads to play killed anim
                hidraHead.GetComponent<HidraHeadTest>().defeated = true;
                Debug.Log("Hidra Has been defeated " + hidraHead.GetComponent<HidraHeadTest>().defeated);

                //FMODUnity.RuntimeManager.PlayOneShot(hidraDeath);

                StopAllCoroutines(); //untested

                Death();
            }
        }
    }
    
    IEnumerator HidraAttackCoroutine()
    {
        yield return new WaitForSeconds(hidraAttackDelay);

        int dice = Random.Range(0, 100);
        //Debug.Log("Dice throw is " + dice);

        if (dice < hidraAttackProbability)
        {
            //Debug.Log("Hidra attacks");

            //elegir una cabeza del array random
            GameObject hidraHead = headsLTR[Random.Range(0, headsLTR.Count)];

            //pasarle a esa cabeza el bool de attack (solo si no esta muerta la cabeza)
            if (hidraHead.GetComponent<HidraHeadTest>().deadHead == false)
            {
                hidraHead.GetComponent<HidraHeadTest>().attack = true;
                HidraAttack();
            }
        }
        //volver a ejectuar ataque (mientras que el monstruo no este muerto)
        if (headsLTR[1].GetComponent<HidraHeadTest>().defeated == false)
        {
            StartCoroutine(HidraAttackCoroutine());
        }
    }

    // Run from HidraHead
    // TODO Should be run by Particle Collision with player.
    public void HidraAttack()
    {
        this.EnemyAttack(hidraAttackDamage);
        Debug.Log("Hidra attacked for " + hidraAttackDamage + " damage. ");
    }
}
