namespace BusinessCardManagerAPI.Data.Models.Contract
{
    public interface ISoftDeletable
    {
        public bool IsDeleted { get; set; }
        public DateTime? DateDeleted { get; set; }

        public void Delete() { IsDeleted = true; DateDeleted = DateTime.Now; }
        public void UndoDelete() { IsDeleted = false; DateDeleted = null; }
    }
}
