using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;
using NDG.BussinesLogic.Providers;

namespace NDG.BussinesLogic.Providers
{
    public class LanguageStrings : INotifyPropertyChanged
    {

        public LanguageStrings()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string arg)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(arg));
            }
        }

        public string CMD_BACK { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_BACK"); } }
        public string CMD_SEND { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_SEND"); } }
        public string OPEN_RESULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OPEN_RESULT"); } }
        public string CMD_DELETE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_DELETE"); } }
        public string CMD_SAVE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_SAVE"); } }
        public string CMD_EXIT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_EXIT"); } }
        public string CMD_NEW_SURVEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_NEW_SURVEY"); } }
        public string CMD_RESULTS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_RESULTS"); } }
        public string CMD_NEXT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_NEXT"); } }
        public string CMD_VIEW { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_VIEW"); } }
        public string CMD_VIEWSENT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_VIEWSENT"); } }
        public string CMD_MARKALL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_MARKALL"); } }
        public string CMD_UNMARKALL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_UNMARKALL"); } }
        public string CMD_CANCEL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_CANCEL"); } }
        public string CMD_MOVETOUNSENT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_MOVETOUNSENT"); } }
        public string CMD_OK { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_OK"); } }
        public string RESULTS_LIST_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESULTS_LIST_TITLE"); } }
        public string SENT_LIST_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SENT_LIST_TITLE"); } }
        public string CATEGORY_LIST_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CATEGORY_LIST_TITLE"); } }
        public string SETTINGS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SETTINGS"); } }
        public string GPS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_GPS"); } }
        public string SUBMIT_SERVER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SUBMIT_SERVER"); } }
        public string YES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_YES"); } }
        public string NO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NO"); } }
        public string SURVEY_LIST_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SURVEY_LIST_TITLE"); } }
        public string MSG_SERVER_CANT_WRITE_RESULTS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MSG_SERVER_CANT_WRITE_RESULTS"); } }
        public string ELOAD_SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ELOAD_SURVEYS"); } }
        public string ELOAD_RESULTS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ELOAD_RESULTS"); } }
        public string EDELETE_RESULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EDELETE_RESULT"); } }
        public string ELEAVE_MIDLET { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ELEAVE_MIDLET"); } }
        public string ELOAD_IMAGES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ELOAD_IMAGES"); } }
        public string EPARSE_SAX { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EPARSE_SAX"); } }
        public string EPARSE_SURVEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EPARSE_SURVEY"); } }
        public string EPARSE_RESULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EPARSE_RESULT"); } }
        public string EPARSE_GENERAL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EPARSE_GENERAL"); } }
        public string EWRITE_RESULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EWRITE_RESULT"); } }
        public string ECREATE_RESULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ECREATE_RESULT"); } }
        public string ERENAME { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ERENAME"); } }
        public string ON { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ON"); } }
        public string OFF { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OFF"); } }
        public string GPS_LOCATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_GPS_LOCATION"); } }
        public string SMALL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SMALL"); } }
        public string MEDIUM { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MEDIUM"); } }
        public string LARGE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LARGE"); } }
        public string LOADING_SURVEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOADING_SURVEY"); } }
        public string PROCESSING { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_PROCESSING"); } }
        public string LOADING_RESULTS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOADING_RESULTS"); } }
        public string SAVING_RESULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SAVING_RESULT"); } }
        public string CMD_OPEN { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_OPEN"); } }
        public string CMD_DOWNLOAD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_DOWNLOAD"); } }
        public string DOWNLOAD_NEW_SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DOWNLOAD_NEW_SURVEYS"); } }
        public string CHECK_NEW_SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CHECK_NEW_SURVEYS"); } }
        public string CONNECTING { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CONNECTING"); } }
        public string DOWNLOADING_NEW_SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DOWNLOADING_NEW_SURVEYS"); } }
        public string EWEBSERVER_ERROR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EWEBSERVER_ERROR"); } }
        public string EDOWNLOAD_FAILED_ERROR_CODE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EDOWNLOAD_FAILED_ERROR_CODE"); } }
        public string EDOWNLOAD_FAILED_INVALID_MIME_TYPE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EDOWNLOAD_FAILED_INVALID_MIME_TYPE"); } }
        public string EDOWNLOAD_FAILED_INVALID_DATA { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EDOWNLOAD_FAILED_INVALID_DATA"); } }
        public string EDOWNLOAD_INCOMPLETED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EDOWNLOAD_INCOMPLETED"); } }
        public string EDOWNLOAD_ACK_ERROR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EDOWNLOAD_ACK_ERROR"); } }
        public string EINVALID_SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EINVALID_SURVEYS"); } }
        public string EINVALID_XML_FILE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EINVALID_XML_FILE"); } }
        public string CMD_DELETE_SURVEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_DELETE_SURVEY"); } }
        public string DELETE_CONFIRMATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DELETE_CONFIRMATION"); } }
        public string DELETE_RESULTS_CONFIRMATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DELETE_RESULTS_CONFIRMATION"); } }
        public string DELETE_SURVEY_CONFIRMATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DELETE_SURVEY_CONFIRMATION"); } }
        public string IMPOSSIBLE_CREATE_ROOTDIR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_IMPOSSIBLE_CREATE_ROOTDIR"); } }
        public string ERROR_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ERROR_TITLE"); } }
        public string GPS_LOCAL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_GPS_LOCAL"); } }
        public string CONNECTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CONNECTED"); } }
        public string LATITUDE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LATITUDE"); } }
        public string LONGITUDE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LONGITUDE"); } }
        public string ALTITUDE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ALTITUDE"); } }
        public string HORIZONTAL_ACCU { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HORIZONTAL_ACCU"); } }
        public string VERTICAL_ACCU { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_VERTICAL_ACCU"); } }
        public string NETWORK_FAILURE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NETWORK_FAILURE"); } }
        public string TRY_AGAIN_LATER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_TRY_AGAIN_LATER"); } }
        public string INTEGER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_INTEGER"); } }
        public string VALUE_GREATER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_VALUE_GREATER"); } }
        public string VALUE_LOWER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_VALUE_LOWER"); } }
        public string SURVEY_NOT_IN_SERVER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SURVEY_NOT_IN_SERVER"); } }
        public string MORE_DETAILS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MORE_DETAILS"); } }
        public string EINVALID_SURVEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EINVALID_SURVEY"); } }
        public string DELETE_RESULT_CONFIRMATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DELETE_RESULT_CONFIRMATION"); } }
        public string QUESTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_QUESTION"); } }
        public string QUESTIONS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_QUESTIONS"); } }
        public string CHECK_FOR_UPDATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CHECK_FOR_UPDATE"); } }
        public string WARNING { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_WARNING"); } }
        public string CMD_VIEW_GPS_DETAILS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_VIEW_GPS_DETAILS"); } }
        public string SURVEY_NOT_DOWNLOADED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SURVEY_NOT_DOWNLOADED"); } }
        public string CMD_TEST_CONNECTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_TEST_CONNECTION"); } }
        public string TESTING_CONNECTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_TESTING_CONNECTION"); } }
        public string CONNECTION_OK { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CONNECTION_OK"); } }
        public string CONNECTION_FAILED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CONNECTION_FAILED"); } }
        public string GPRS_LABEL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_GPRS_LABEL"); } }
        public string CMD_HIDE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_HIDE"); } }
        public string CONNECTION_WAIT_FOR_ACK { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CONNECTION_WAIT_FOR_ACK"); } }
        public string CMD_SELECT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_SELECT"); } }
        public string CMD_OPTIONS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_OPTIONS"); } }
        public string CHECK_FOR_UPDATE_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CHECK_FOR_UPDATE_TITLE"); } }
        public string NDG_UPDATED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_UPDATED"); } }
        public string NDG_NOT_UPDATED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_NOT_UPDATED"); } }
        public string DECIMAL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DECIMAL"); } }
        public string DATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DATE"); } }
        public string NEW_INTERVIEW { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NEW_INTERVIEW"); } }
        public string EDITING { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EDITING"); } }
        public string TRY_AGAIN { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_TRY_AGAIN"); } }
        public string REGISTERING_APP { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_REGISTERING_APP"); } }
        public string REGISTRATION_DONE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_REGISTRATION_DONE"); } }
        public string APP_REGISTERED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APP_REGISTERED"); } }
        public string NDG_CHECK_NTWRK { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_CHECK_NTWRK"); } }
        public string NDG_NO_ROUTE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_NO_ROUTE"); } }
        public string NDG_SOFTWARE_ABORT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_SOFTWARE_ABORT"); } }
        public string NDG_CONNECTION_REFUSED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_CONNECTION_REFUSED"); } }
        public string NDG_PERMISSION_DENIED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_PERMISSION_DENIED"); } }
        public string NDG_NETWORK_DOWN { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_NETWORK_DOWN"); } }
        public string NDG_NETWORK_UNREACHABLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_NETWORK_UNREACHABLE"); } }
        public string NDG_CONNECTION_TIMEOUT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_CONNECTION_TIMEOUT"); } }
        public string NDG_NETWORK_UNAVAILABLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_NETWORK_UNAVAILABLE"); } }
        public string NDG_HOST_NOT_FOUND { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_HOST_NOT_FOUND"); } }
        public string HTTP_NOT_FOUND { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HTTP_NOT_FOUND"); } }
        public string HTTP_FORBIDDEN { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HTTP_FORBIDDEN"); } }
        public string HTTP_BAD_REQUEST { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HTTP_BAD_REQUEST"); } }
        public string HTTP_UNAUTHORIZED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HTTP_UNAUTHORIZED"); } }
        public string HTTP_INTERNAL_ERROR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HTTP_INTERNAL_ERROR"); } }
        public string HTTP_OVERLOADED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HTTP_OVERLOADED"); } }
        public string CHECK_SERVER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CHECK_SERVER"); } }
        public string FAILED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_FAILED"); } }
        public string NDG_NO_DNS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_NO_DNS"); } }
        public string NDG_NO_LOCATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NDG_NO_LOCATION"); } }
        public string IMEI_ALREADY_EXISTS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_IMEI_ALREADY_EXISTS"); } }
        public string MSISDN_NOT_FOUND { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MSISDN_NOT_FOUND"); } }
        public string REGISTRATION_FAILURE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_REGISTRATION_FAILURE"); } }
        public string MSISDN_ALREADY_REGISTERED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MSISDN_ALREADY_REGISTERED"); } }
        public string WIRELESS_INTERFACE_ERROR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_WIRELESS_INTERFACE_ERROR"); } }
        public string THERE_ARE_NO_NEW_SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_THERE_ARE_NO_NEW_SURVEYS"); } }
        public string NOTPROPERINTEGER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NOTPROPERINTEGER"); } }
        public string NOTPROPERDECIMAL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NOTPROPERDECIMAL"); } }
        public string RESTORE_DEFAULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESTORE_DEFAULT"); } }
        public string RELOAD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RELOAD"); } }
        public string LOADING_STYLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOADING_STYLE"); } }
        public string LOADING_STYLE_ERROR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOADING_STYLE_ERROR"); } }
        public string UI_CUSTOMIZE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UI_CUSTOMIZE"); } }
        public string SELECTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SELECTED"); } }
        public string UNSELECTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UNSELECTED"); } }
        public string PREVIEW { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_PREVIEW"); } }
        public string ELEMENT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ELEMENT"); } }
        public string OBJECT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OBJECT"); } }
        public string LIST { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LIST"); } }
        public string MENU { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MENU"); } }
        public string DIALOG_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DIALOG_TITLE"); } }
        public string BG_SELECTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_BG_SELECTED"); } }
        public string BG_UNSELECTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_BG_UNSELECTED"); } }
        public string FONT_SELECTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_FONT_SELECTED"); } }
        public string FONT_UNSELECTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_FONT_UNSELECTED"); } }
        public string ACCESS_DENIED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ACCESS_DENIED"); } }
        public string LOAD_FROM_FILE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOAD_FROM_FILE"); } }
        public string MEMORY_OUT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MEMORY_OUT"); } }
        public string DEFAULT_PHOTO_DIR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DEFAULT_PHOTO_DIR"); } }
        public string CAPTURE_PHOTO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CAPTURE_PHOTO"); } }
        public string DELETE_PHOTO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DELETE_PHOTO"); } }
        public string PHOTO_RESOLUTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_PHOTO_RESOLUTION"); } }
        public string SHOW_PHOTO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SHOW_PHOTO"); } }
        public string TAKE_PHOTO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_TAKE_PHOTO"); } }
        public string JUST_SAVE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_JUST_SAVE"); } }
        public string UI_PREFERENCES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UI_PREFERENCES"); } }
        public string GEO_TAGGING_CONF { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_GEO_TAGGING_CONF"); } }
        public string MAX_IMG_NO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MAX_IMG_NO"); } }
        public string RESOLUTIONS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESOLUTIONS"); } }
        public string DEFAULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DEFAULT"); } }
        public string HIGH_CONTRAST { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_HIGH_CONTRAST"); } }
        public string CUSTOM { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CUSTOM"); } }
        public string STYLES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_STYLES"); } }
        public string TIME_FORMAT_ERROR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_TIME_FORMAT_ERROR"); } }
        public string DATE_FORMAT_ERROR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DATE_FORMAT_ERROR"); } }
        public string DISCARD_CHANGES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DISCARD_CHANGES"); } }
        public string ENCRYPTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ENCRYPTION"); } }
        public string ENCRYPTION_ENABLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ENCRYPTION_ENABLE"); } }
        public string ENCRYPTION_WITH_PASSWORD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ENCRYPTION_WITH_PASSWORD"); } }
        public string ENCRYPTION_PASSWORD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ENCRYPTION_PASSWORD"); } }
        public string DECRYPTION_FAILED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DECRYPTION_FAILED"); } }
        public string EMPTY_KEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EMPTY_KEY"); } }
        public string WRONG_KEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_WRONG_KEY"); } }
        public string SAVE_SURVEY_QUESTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SAVE_SURVEY_QUESTION"); } }
        public string ADD_LOCATION_FAILURE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ADD_LOCATION_FAILURE"); } }
        public string LOCATION_OUT_OF_DATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOCATION_OUT_OF_DATE"); } }
        public string LOCATION_OUT_OF_DATE_WARN { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOCATION_OUT_OF_DATE_WARN"); } }
        public string NEWUI_NOKIA_DATA_GATHERING { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NEWUI_NOKIA_DATA_GATHERING"); } }
        public string DATE_FORMAT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DATE_FORMAT"); } }
        public string AVAILABLE_DATE_FORMAT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_AVAILABLE_DATE_FORMAT"); } }
        public string OR_FORM_LOADING_FAILURE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OR_FORM_LOADING_FAILURE"); } }
        public string OR_VALID_INPUT_FROM { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OR_VALID_INPUT_FROM"); } }
        public string TO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_TO"); } }
        public string OR_INVALID_INPUT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OR_INVALID_INPUT"); } }
        public string SEND_ERRORS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SEND_ERRORS"); } }
        public string RESULT_NOT_SENT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESULT_NOT_SENT"); } }
        public string REMOVE_CATEGORIES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_REMOVE_CATEGORIES"); } }
        public string CATEGORIES_LIMIT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CATEGORIES_LIMIT"); } }
        public string ADD_CATEGORIES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ADD_CATEGORIES"); } }
        public string ADD_ADDITIONAL_COPIES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ADD_ADDITIONAL_COPIES"); } }
        public string FAIL_IMAGE_SAVE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_FAIL_IMAGE_SAVE"); } }
        public string OUT_OF_MEMORY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OUT_OF_MEMORY"); } }
        public string NOT_ENOUGH_MEMORY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NOT_ENOUGH_MEMORY"); } }
        public string CATEGORY_DISABLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CATEGORY_DISABLE"); } }
        public string SUB_CATEGORY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SUB_CATEGORY"); } }
        public string AVAILABLE_STYLES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_AVAILABLE_STYLES"); } }
        public string AVAILABLE_FONT_SIZE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_AVAILABLE_FONT_SIZE"); } }
        public string UNSUPPORTED_TYPE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UNSUPPORTED_TYPE"); } }
        public string SURVEY_LOCALIZED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SURVEY_LOCALIZED"); } }
        public string CORRUPTED_SURVEY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CORRUPTED_SURVEY"); } }
        public string ONE_SURVEY_CORRUPTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ONE_SURVEY_CORRUPTED"); } }
        public string CONDITION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CONDITION"); } }
        public string EFAILED_LOAD_IMAGE_LIMITED_DEVICE_RESOURCES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_EFAILED_LOAD_IMAGE_LIMITED_DEVICE_RESOURCES"); } }
        public string SHOW_CHOICES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SHOW_CHOICES"); } }
        public string NATIVE_RESOLUTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NATIVE_RESOLUTION"); } }
        public string CMD_COLORS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_COLORS"); } }
        public string CMD_SIZES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_SIZES"); } }
        public string CMD_CLEAR { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CMD_CLEAR"); } }
        public string RESTART { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESTART"); } }
        public string MSG_RESTART_NEEDED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MSG_RESTART_NEEDED"); } }
        public string MSG_NEW_LANGUAGE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MSG_NEW_LANGUAGE"); } }
        public string LANGUAGE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LANGUAGE"); } }
        public string DOWNLOAD_LOCALE_FAILED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DOWNLOAD_LOCALE_FAILED"); } }
        public string DOWNLOAD_LANG_LIST_FAILED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DOWNLOAD_LANG_LIST_FAILED"); } }
        public string CHECK { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CHECK"); } }
        public string WARNING_TITLE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_WARNING_TITLE"); } }
        public string NEW_LANGUAGES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NEW_LANGUAGES"); } }
        public string NO_NEW_LANGUAGE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NO_NEW_LANGUAGE"); } }
        public string UPDATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UPDATE"); } }
        public string UPDATE_SUCCESS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UPDATE_SUCCESS"); } }
        public string LOG_IN { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_LOG_IN"); } }
        public string SERVER_ADDRESS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SERVER_ADDRESS"); } }
        public string ABOUT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ABOUT"); } }
        public string WRONG_SERVER_ADDRESS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_WRONG_SERVER_ADDRESS"); } }
        public string WRONG_PASSWORD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_WRONG_PASSWORD"); } }
        public string CANNOT_LOCATE_SERVER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_CANNOT_LOCATE_SERVER"); } }
        public string ERROR_LOGIN_DB { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ERROR_LOGIN_DB"); } }
        public string MY_SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_MY_SURVEYS"); } }
        public string SEARCH_WITH_DOTS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SEARCH_WITH_DOTS"); } }
        public string FILTER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_FILTER"); } }
        public string SURVEY_DELETED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SURVEY_DELETED"); } }
        public string ERROR_SURVEY_DELETE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ERROR_SURVEY_DELETE"); } }
        public string SAVED_RESPONSES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SAVED_RESPONSES"); } }
        public string RESPOSE_DELETED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESPOSE_DELETED"); } }
        public string ERROR_RESPONSE_DELETE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ERROR_RESPONSE_DELETE"); } }
        public string SUBMITTED_RESPONSES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SUBMITTED_RESPONSES"); } }
        public string SEARCH { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SEARCH"); } }
        public string RESOLUTION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESOLUTION"); } }
        public string RESTORE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESTORE"); } }
        public string DETAILS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_DETAILS"); } }
        public string NO_RESPONSES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NO_RESPONSES"); } }
        public string SUBMITTED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SUBMITTED"); } }
        public string IN_PROGRESS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_IN_PROGRESS"); } }
        public string COMPLETE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_COMPLETE"); } }
        public string NEW { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NEW"); } }
        public string RESPONSE_GENERATED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_RESPONSE_GENERATED"); } }
        public string START_DATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_START_DATE"); } }
        public string END_DATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_END_DATE"); } }
        public string FILTER_RESPONSES { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_FILTER_RESPONSES"); } }
        public string BY_DATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_BY_DATE"); } }
        public string SAVED { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SAVED"); } }
        public string REFRESH { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_REFRESH"); } }
        public string INFO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_INFO"); } }
        public string VIEW_ALL { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_VIEW_ALL"); } }
        public string TOC { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_TOC"); } }
        public string NO_RESPONSES_CATEGORY { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NO_RESPONSES_CATEGORY"); } }
        public string NAME_YOUR_RESPONSE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_NAME_YOUR_RESPONSE"); } }
        public string SEARCH_RESULTS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SEARCH_RESULTS"); } }
        public string SURVEYS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SURVEYS"); } }
        public string INCOMPLETE_UPLOAD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_INCOMPLETE_UPLOAD"); } }

        public string USERNAME { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_USERNAME"); } }
        public string PASSWORD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_PASSWORD"); } }
        public string UPLOAD { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UPLOAD"); } }
        public string SUBMITTED_SUCCESS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_SUBMITTED_SUCCESS"); } }
        public string UPLOAD_CONFIRMATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_UPLOAD_CONFIRMATION"); } }
        public string ALLOW_GPS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_ALLOW_GPS"); } }
        public string REVERT_SETTINGS_CONFIRMATION { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_REVERT_SETTINGS_CONFIRMATION"); } }
        public string OPEN_DUPLICATE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_OPEN_DUPLICATE"); } }

        public string APPBAR_CMD_OK { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_CMD_OK"); } }
        public string APPBAR_SEARCH { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_SEARCH"); } }
        public string APPBAR_REFRESH { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_REFRESH"); } }
        public string APPBAR_SETTINGS { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_SETTINGS"); } }
        public string APPBAR_INFO { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_INFO"); } }
        public string APPBAR_CMD_SAVE { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_CMD_SAVE"); } }
        public string APPBAR_LOG_IN { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_LOG_IN"); } }
        public string APPBAR_FILTER { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_FILTER"); } }
        public string APPBAR_RESTORE_DEFAULT { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_RESTORE_DEFAULT"); } }
        public string APPBAR_NEW { get { return LanguageProvider.CurrentLanguage.TryGetString("QTJ_APPBAR_NEW"); } }

        public void RaiseAllPropertyChanged()
        {
            var properties = this.GetType().GetProperties();
            foreach (var property in properties)
            {
                RaisePropertyChanged(property.Name);
            }
        }
    }
}
