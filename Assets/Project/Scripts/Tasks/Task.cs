using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Task
{
    public string taskName;
    public string TaskName
    {
        get { return taskName; }
    }
    public virtual bool IsDragAllowed { get{ return true; }}

    public bool isStarted = false;
    protected float createTime;
    public float taskDeadline;
    protected List<Task> subtasks = new List<Task>();
    abstract public void TaskStart();
    abstract public void Actions();
    abstract public void TaskInterrupt();
    abstract public void TaskEnd();
    abstract public void TaskFail();

    abstract public bool isDone();
    abstract public bool isFailed();

    protected void SubtasksExecution()
    {
        if (subtasks.Count == 0) return;
        
         if (subtasks[0].isFailed() || subtasks[0].taskDeadline >= 0 && Time.time > subtasks[0].taskDeadline)
        {
            subtasks[0].TaskFail();
            subtasks.Remove(subtasks[0]);
            return;
        }
        if (subtasks[0].isDone())
        {
            subtasks[0].TaskEnd();
            subtasks.Remove(subtasks[0]);
            return;
        }
        if (!subtasks[0].isStarted)
        {
            subtasks[0].TaskStart();
            subtasks[0].isStarted = true;
        }
        subtasks[0].Actions();
    }

    public void SetTimeLimit(float timeLimit)
    {
        this.taskDeadline = timeLimit >= 0 ? Time.time + timeLimit : -1;
    }

    public Task(string taskName, float timeLimit)
    {
        createTime = Time.time;
        this.taskName = taskName;
        this.taskDeadline = timeLimit >= 0 ? Time.time + timeLimit : -1;
    }
}
