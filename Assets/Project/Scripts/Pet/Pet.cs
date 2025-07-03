using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using TMPro;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Pet : MonoBehaviour, IObjectFunction
{
    
    public string petName = "dumb slime";
    private System.DateTime createDate;
    public System.DateTime CreateDate {get{ return createDate; } set{ createDate = value; }}
    private PetStats stats = new PetStats();
    public PetStats Stats { get { return stats; } set { stats = value; } }

    [Header("Parameters")]

    [SerializeField] private float moveSpeed = 2;
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }
    [SerializeField] private float speedMultiplier = 1;
    public float SpeedMultiplier { get{ return speedMultiplier; } set{ speedMultiplier = value; }}
    [Header("Content")]
    [SerializeField] private AudioClip pooSound;
    [SerializeField] private AudioClip eatSound;
    public AudioClip EatSound { get { return eatSound; } }
    [SerializeField] private GameObject poopPrefab;

    [Header("Links")]
    public Transform bed;
    public TextMeshProUGUI tasksText;
    private Animator animator;
    public Animator Animator
    {
        get { if (animator == null) animator = GetComponent<Animator>(); return animator; }
    }

    //variables
    private float breakUntil = 0;

    [SerializeField]
    private List<Task> tasks = new List<Task>();
    public int TaskCount { get {return tasks.Count; }}
    [SerializeField]
    private List<String> taskHistory = new List<string>();

    private List<string> idleRequests = new List<string>();

    public static Pet Instance { get; private set; }

    public const float middleTick = 15;
    public const float largeTick = 1800;
    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SaveManger.Load();
        StartCoroutine(Autosave());
        StartCoroutine(PetSmallTick());
        StartCoroutine(PetLargeTick());
    }
    private void Update()
    {
        if (Time.time > breakUntil && idleRequests.Count <= 0)
        {
            if (tasks.Count > 0) TaskExecution();
            else
            {
                Task randTask = TaskGenerator.GetRandomTask();
                AddTask(randTask);
            }
        }
    }
    private IEnumerator PetSmallTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(middleTick / Time.timeScale);
            if (tasks.Count == 0 || GetActiveTask().taskName != "Sleep")
            {
                Stats.Satiety -= 0.25f;
                Stats.Cleanliness -= 0.1f;
                Stats.Love -= 0.1f;
                Stats.Boredom -= 1f;
                Stats.Cheerfulness -= 0.10f;
            }
            else
            {
                Stats.Satiety -= 0.5f;
                Stats.Cleanliness -= 0.5f;
            }
            Stats.Satiety = Math.Clamp(Stats.Satiety, 0, 10);
            Stats.Cleanliness = Math.Clamp(Stats.Cleanliness, 0, 10);
            Stats.Love = Math.Clamp(Stats.Love, 0, 10);
            Stats.Boredom = Math.Clamp(Stats.Boredom, 0, 10);
            Stats.Cheerfulness = Math.Clamp(Stats.Cheerfulness, 0, 10);

            PetMenu.instance.UpdateStatSliders();

            ParticleManager pm = ParticleManager.Instance;
            if (Stats.Cleanliness <= 2f && !pm.PoopSmell.isPlaying)
            {
                pm.PoopSmell.Play();
            }
            else if (Stats.Cleanliness > 2f && pm.PoopSmell.isPlaying)
            {
                pm.PoopSmell.Stop();
            }
        }
    }
    private IEnumerator PetLargeTick()
    {
        while (true)
        {
            yield return new WaitForSeconds(largeTick / Time.timeScale);
            if (Stats.Satiety <= 0.5f && Stats.Cleanliness <= 0.5f && Stats.Love <= 0.5f && Stats.Boredom <= 5)
            {
                Stats.Happiness = Stats.Happiness - 0.25f;
            }
            else
            {
                Stats.Happiness = Stats.Happiness + 0.5f;
                Stats.Expirience += 3;
            }

            if (Stats.StoriesQueue > 0 && tasks.Count == 0 || Stats.StoriesQueue > 0 && tasks[0].TaskName != "Story")
            {
                if (StoriesWindow.Instance.GetArrayOfAvailableStories().Length > 0)
                {
                    Task storyTask = new GoGetAStoryTask("Story", -1);
                    AddTask(storyTask);
                }
            }
        }
        
    }
    public void RMB_Click()
    {
        if (tasks.Count == 0 || tasks[0].taskName.ToLower() != "sleep")
        {
                PetMenu.instance.Open();
        }
        else
        {
            BedMenu.instance.Open();
        }
    }
    private IEnumerator Autosave()
    {
        while (true)
        {
            yield return new WaitForSeconds(30f);
            SaveManger.Save();
        }
    }
    private IEnumerator PooC()
    {
        float rand = Random.Range(1f, 5f);
        yield return new WaitForSeconds(rand);
        Stats.PoopBar -= 3;
        Stats.Cleanliness = Stats.Cleanliness - 1;
        GameObject poopObj = Instantiate(poopPrefab, transform.position, quaternion.identity);
        RoomObject PoopRoomObj = poopObj.GetComponent<RoomObject>();
        FurnitureBox.instance.AddFurnitureToRoom(PoopRoomObj);

        SoundManager.instance.PlaySound(pooSound, 0.5f, SoundType.Main);

        if (Stats.PoopBar >= 3) StartCoroutine(PooC());
    }
    public void Poo()
    {
        StartCoroutine(PooC());
    }
    private void TaskExecution()
    {
        if (tasks[0].isFailed() || tasks[0].taskDeadline >= 0 && Time.time > tasks[0].taskDeadline)
        {
            tasks[0].TaskFail();
            RemoveTask(tasks[0]);
            return;
        }
        if (tasks[0].isDone())
        {
            tasks[0].TaskEnd();
            RemoveTask(tasks[0]);
            //Break(Random.Range(1,10));
            SaySomething();
            return;
        }
        if (!tasks[0].isStarted)
        {
            tasks[0].TaskStart();
            tasks[0].isStarted = true;
        }
        tasks[0].Actions();
    }
    private void SaySomething()
    {
        int rand = Random.Range(0, 100);
        if (rand <= 5)
        {
            rand = Random.Range(0, 100);
            switch (rand)
            {
                case int n when (n <= 5):
                    MonologueWindow.instance.RandomHatMonologue();
                    break;
                default:
                    MonologueWindow.instance.RandomIdleMonologue();
                    break;
            }
        }
    }

    private void TasksOtput()
    {
        String text = "Tasks:";
        for (int i = 0; i < tasks.Count; i++)
        {
            text += "\n" + (i + 1) + ". " + tasks[i].TaskName;
        }

        tasksText.text = text;
    }
    public void AddTask(Task task)
    {
        tasks.Add(task);
        TasksOtput();

        //string taskString = ("[ " + DateTime.Now.ToString("yy-MM-dd | hh:mm:ss") + "]: " + task.taskName);
        //taskHistory.Add(taskString);
    }
    public void AddTask(Task task, int insertIndex)
    {
        if (insertIndex == 0 && tasks.Count > 0)
        {
            tasks[0].TaskInterrupt();
        }
        tasks.Insert(insertIndex, task);
        TasksOtput();
    }
    public void RemoveTask(Task task)
    {
        tasks.Remove(task);
        TasksOtput();
    }
    public void CancelActiveTask()
    {
        tasks[0].TaskFail();
        tasks.Remove(tasks[0]);
        TasksOtput();
    }
    public Task GetActiveTask()
    {
        if (tasks.Count > 0)
            return tasks[0];
        else
            return null;
    }
    public void ClearTasks()
    {
        tasks.Clear();
        TasksOtput();
    }
 
    public void Break(float time)
    {
        breakUntil = Time.time + time;
        //Debug.Log("break until " + breakUntil);
    }

    public void CancelBreak()
    {
        breakUntil = 0;
    }
    public void AddIdleRequest(String requestTitle)
    {
        if (idleRequests.Contains(requestTitle)) return;

        idleRequests.Add(requestTitle);
        Animator.SetBool("IsWalk", false);
    }
    public void RemoveIdleRequest(String requestTitle)
    {
        if (!idleRequests.Contains(requestTitle)) return;

        idleRequests.Remove(requestTitle);
        if (idleRequests.Count == 0 && tasks.Count > 0)
            tasks[0].TaskStart();
    }

    public bool IsThereIdleRequest(String requestTitle)
    {
        return idleRequests.Contains(requestTitle) ? true : false;
    }
}
