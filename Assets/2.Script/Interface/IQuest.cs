public interface IQuest
{
    int QuestID { get; }
    bool IQuestClear { get; }

    void SetQuestClear();
}
