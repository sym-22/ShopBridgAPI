using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Configuration
{
    /// <summary>
    ///     static class holding global constants
    /// </summary>
    public static class Constants
    {
        public const string ITEM_NOT_EXIST_MESSAGE = "An item with id = {0} does not exist.";
        public const string INTERNAL_SERVER_ERROR_MESSAGE = "Something went wrong. Please connect with system administrator.";
        public const string INVALID_PAGE_NUMBER_MESSAGE = "Invalid value for page number.";
    }
}
