using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

public class SlotMachine : RoomObject, IObjectFunction
{
    [Header("Settings")]
    [SerializeField] private int maxSpinCountForSlot;
    [SerializeField] private float spreadForce = 8;
    public Sprite[] PossibleIcons;
    public SlotOutcome[] Outcomes;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer[] _slotDisplays;
    [SerializeField] private GameObject poopPrefab;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] private GameObject[] foodPrefabs;

    [SerializeField] private AudioClip cancelSound;
    [SerializeField] private AudioClip rollSound;
    [SerializeField] private AudioClip winSound;
    [SerializeField] private AudioClip spinStartSound;

    private List<Sprite> _currentResult = new List<Sprite>();
    private bool coolDown = false;
    private int currentIconIndex = 0;

    private void Start()
    {
        ResetSlots();
    }
    public void RMB_Click()
    {
        Spin();
    }
    public void Spin()
    {
        if (coolDown) return;
        PetStats stats = Pet.Instance.Stats;
        if (stats.Coins > 0)
        {
            stats.Coins--;
            StartCoroutine(SpinRoutine());
            animator.SetTrigger("Spin");
            SoundManager.instance.PlaySound(spinStartSound, 0, SoundType.Main);
        }
        else
        {
            SoundManager.instance.PlaySound(cancelSound, 0, SoundType.Main);
        }
    }
    public void OnCoins()
    {
        StartCoroutine(SpawnObjects(coinPrefab));
    }
    public void OnPoop()
    {
        StartCoroutine(SpawnObjects(poopPrefab));
    }
    public void OnSeven()
    {
        StartCoroutine(SpawnObjects(foodPrefabs));
    }

    private IEnumerator SpawnObjects(params GameObject[] objectReferences)
    {
        int count = Random.Range(2, 15);
        for (int i = 0; i < count; i++)
        {
            GameObject newObject = Instantiate(objectReferences[i % objectReferences.Length], transform.position, Quaternion.identity);
            RoomObject newRoomObject = newObject.GetComponent<RoomObject>();
            FurnitureBox.instance.AddFurnitureToRoom(newRoomObject);


            if (newObject.GetComponent<Rigidbody2D>())
            {
                Rigidbody2D rb = newObject.GetComponent<Rigidbody2D>();

                Vector2 direction = new Vector2(Random.Range(-spreadForce, spreadForce), Random.Range(-spreadForce, spreadForce));
                rb.velocity = direction;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    private IEnumerator SpinRoutine()
    {
        coolDown = true;
        ResetSlots();
        _currentResult.Clear();

        for (int i = 0; i < 3; i++)
        {
            yield return StartCoroutine(SpinSingleSlot(i));
        }

        CheckOutcomes();
        coolDown = false;
    }

    private IEnumerator SpinSingleSlot(int slotIndex)
    {
        int targetSpinCount = Random.Range(3, maxSpinCountForSlot);
        int spinCount = 0;

        while (spinCount < targetSpinCount)
        {
            _slotDisplays[slotIndex].sprite = GetNextIcon();
            spinCount++;
            SoundManager.instance.PlaySound(rollSound, 0.2f, SoundType.Main);

            yield return new WaitForSeconds(0.1f);
        }

        Sprite finalIcon = GetNextIcon();
        _slotDisplays[slotIndex].sprite = finalIcon;
        _currentResult.Add(finalIcon);
    }

    private Sprite GetNextIcon()
    {
        currentIconIndex++;
        int index = currentIconIndex % PossibleIcons.Length;
        return PossibleIcons[index];
    }

    private void CheckOutcomes()
    {
        foreach (var outcome in Outcomes)
        {
            if (CheckCombination(outcome.Combination, outcome.OrderSensitive))
            {
                outcome.OnTrigger.Invoke();
                SoundManager.instance.PlaySound(winSound, 0, SoundType.Main);

            }
        }
    }

    private bool CheckCombination(Sprite[] target, bool checkOrder)
    {
        if (target.Length != 3) return false;

        if (checkOrder)
        {
            for (int i = 0; i < 3; i++)
            {
                if (target[i] != _currentResult[i]) return false;
            }
            return true;
        }
        else
        {
            List<Sprite> temp = new List<Sprite>(_currentResult);
            foreach (Sprite sprite in target)
            {
                if (!temp.Contains(sprite)) return false;
                temp.Remove(sprite);
            }
            return true;
        }
    }

    private void ResetSlots()
    {
        foreach (SpriteRenderer slot in _slotDisplays)
        {
            slot.sprite = null;
        }
    }

    public override Task ObjectTask()
    {
        if (Random.Range(0, 10) == 0)
        {
            Task task = new PlaySlotMachineTask("PlaySlotMachine", 10f, this);
            return task;
        }
        else
        {
            return base.ObjectTask();
        }
    }
}