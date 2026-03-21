public class MinionTaskData
{
    public MinionTask TaskType;
    public GridCell TargetCell;
    public int Priority;

    public MinionTaskData(MinionTask taskType, GridCell targetCell, int priority)
    {
        TaskType = taskType;
        TargetCell = targetCell;
        Priority = priority;
    }
}