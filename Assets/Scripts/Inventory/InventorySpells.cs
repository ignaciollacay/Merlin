using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof (Counter))] // TODO Is this really required or an extension?
public class InventorySpells : MonoBehaviour // Use SO instead?
{
    [SerializeField] private InventorySpellSO inventory;
    public Counter counter;

    private void Start()
    {
        //PhraseRecognition.OnPhraseRecognized += GetNextSpell & OnSpellChange.Invoke();
    }

    private void Awake() => counter = GetComponent<Counter>();
    public SpellSO GetCurrentSpell() => inventory.spells[counter.count];
    public SpellSO GetNextSpell() => inventory.spells[counter.Next(inventory.spells.Count)];
    public SpellSO GetPreviousSpell() => inventory.spells[counter.Next(inventory.spells.Count)];

    public int GetCount() => inventory.spells.Count;
    public List<SpellSO> GetList() => inventory.spells;
    public void AddItem(SpellSO spell) => inventory.spells.Add(spell);
}
