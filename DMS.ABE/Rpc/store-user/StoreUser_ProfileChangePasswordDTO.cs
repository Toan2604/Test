namespace DMS.ABE.Rpc.store_user
{
    public class StoreUser_ProfileChangePasswordDTO
    {
        public long Id { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
