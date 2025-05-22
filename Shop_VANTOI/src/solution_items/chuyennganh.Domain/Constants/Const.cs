namespace chuyennganh.Domain.Constants
{
    /// <summary>
    /// Contain all constant for application
    /// </summary>
    public static class Const
    {
        #region Connection

        public const string CONN_CONFIG_SQL = "DbSqlServer";


        #endregion

        #region rabbitMQ

        public const string BROKER_CONFIG = "MessageBroker";
        public const string BROKER_HOST = "Host";
        public const string BROKER_USERNAME = "Username";
        public const string BROKER_PASSWORD = "Password";

        #endregion

        #region FileUpload
        public const string UPLOADED_SETTINGS = "UploadSettings";
        public const string DOMAIN_HOSTS = "HostSettings";
        public const string OUTPUTFILE_UPLOADED = "asset/images/uploaded";


        public const string FILENAME_PRODUCT = "img-{0}";
        public const string FILENAME_CATEGORY = "img-category-{0}";

        public const string FILENAME_CUSTOMER = "img-customer-{0}";

        public const string FILENAME_CUSTOMER_ADDRESS = "img-cus-address-{0}";
        //public const string FILENAME_EMPLOYEE_VERIFY = "img-emp-verify-{0}-{1}";

        //public const string FILENAME_PRO_SUPPLIER = "img-supplier-{0}";
        //public const string FILENAME_PRO_SUPPLIER_VERIFY = "img-sup-verify-{0}-{1}";

        #endregion

        #region
        public const string VN_CONFIG = "VnPay";
        #endregion
        public const string SOCIAL = "Facebook_Google";
    }
}