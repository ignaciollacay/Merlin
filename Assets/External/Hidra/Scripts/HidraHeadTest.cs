using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HidraHeadTest : MonoBehaviour
{
    [Header("Anim Properties")]
    public bool multiple = false;
    public bool marked = false;
    public bool attack = false;
    public bool deadHead = false;
    public bool defeated = false;

    //SOUND EVENT EMITTERS
    [Header("Sound Events")]
    public string arrowHitHidra = "FMOD Event Path";
    public string recieveDamage = "FMOD Event Path";
    public string hidraHeadDeath = "FMOD Event Path";
    public string hidraAttack = "FMOD Event Path";

    //private HidraTest _hidraTest;
    [SerializeField] HidraTest _hidraTest;
    [SerializeField] GameObject attackVfx;

    //HidraTest _hidraTest;
    Animator animator;
    
    
    private void Awake()
    {
        //innecesario hacer correr cosas en el juego si lo puedo settear manualmente. no?
        //_hidraTest = GetComponentInParent<HidraTest>();

        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        StartCoroutine(Multiples());
        StartCoroutine(MarkHead());
        StartCoroutine(HidraDefeated());
        StartCoroutine(HidraAttack());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Arrow")
        {
            HeadKilled();
        }
    }

    void HeadKilled()
    {
        deadHead = true;

        //FMODUnity.RuntimeManager.PlayOneShot(arrowHitHidra);

        if (multiple)
        {
            animator.SetBool("isMultiple", true);

            //FMODUnity.RuntimeManager.PlayOneShot(hidraHeadDeath);
        }
        if (!multiple) //innecesario
        {
            animator.SetBool("isMultiple", false);
            //aca tendria que hacer que isShot pase a falso
            ////o mejor, crear una instance del objeto

            //FMODUnity.RuntimeManager.PlayOneShot(recieveDamage);

            StartCoroutine(HeadRegeneration());
        }
        if (marked)
        {
            animator.SetBool("isMarked", false);
            marked = false;
            //calculate the next multiplier and pass the mark
            _hidraTest.HeadMark();
            //_hidraTest.currentMultiplier++;
            //_hidraTest.HeadMark();
        }

        animator.SetBool("isShot", true);

        if (marked || multiple)
        {
            _hidraTest.AllHeadsKilled();
        }
    }

    IEnumerator HidraAttack()
    {
        if ((!defeated) && (!deadHead)) //corroboro devuelta que no este muerta la cabeza ni la hidra
        {
            yield return new WaitUntil(() => attack);
            animator.SetBool("isAttacking", true);

            //cuando termina la animacion, hay que volver a pasarlo a falso.
            yield return new WaitForSeconds(5); // TBD -- tiempo de animacion
            animator.SetBool("isAttacking", false);

            //FMODUnity.RuntimeManager.PlayOneShot(hidraAttack);
        }
    }

    IEnumerator Multiples()
    {
        yield return new WaitUntil(() => multiple);
        animator.SetBool("isMultiple", true);
    }

    IEnumerator MarkHead()
    {
        yield return new WaitUntil(() => marked);
        animator.SetBool("isMarked", true);
    }

    IEnumerator HidraDefeated()
    {
        yield return new WaitUntil(() => defeated);
        animator.SetBool("isDefeated", true);


        if (multiple == false)
        {
            HeadKilled(); //necesito pasar por el death de las cabezas no-multiplos
        }
    }

    IEnumerator HeadRegeneration()
    {
        yield return new WaitForSeconds(1); //esto deberia ser cuando termina la anim.
        animator.SetBool("isShot", false);
        deadHead = false;
    }

    //Nasty fix esta solucion.
    void AttackVfx()
    {
        attackVfx.SetActive(true);
        StartCoroutine(WaitForSeconds(1));
    }
    IEnumerator WaitForSeconds(int waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        attackVfx.SetActive(false);
    }
}
