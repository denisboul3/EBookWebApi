using bookstore.api.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Drawing.Printing;

namespace bookstore.api.ResponseMessage;
public class ResponseMessage<TModel>
{
    public Dictionary<string, object> Result { get; set; }

    public TModel Data { get; set; }

    public ResponseMessage() => Result = new Dictionary<string, object>();

    public void PutError(ErrorCode errorCode, string error)
    {
        Result.Add("errorCode", (int)errorCode); 
        Result.Add("Message", error);
    }

    public bool NoErrors() => Result.Count == 0 || Result.IsNull();
}


public enum ErrorCode
{
    BOOK_NOT_FOUND = 100,
    BOOK_NOT_FOUND_EXCEPTION,
    BOOK_COULD_NOT_BE_CREATED_EXCEPTION,
    BOOK_NAME_ALREADY_EXISTS,
    BOOK_MODIFY_EXCEPTION,
    BOOK_DELETE_EXCEPTION,

    PRICE_ID_DOES_NOT_EXIST = 200,
    PRICE_DAY_NOT_VALID,
    PRICE_MODIFY_EXCEPTION,

    BROWSE_BOOKS_NOT_FOUND = 300,
    BROWSE_BOOKS_EXCEPTION,
    BOOK_ALREADY_EXISTS,

    USER_ASSIGN_ROLE_NOT_FOUND = 400,
    USER_NOT_FOUND,

    ROLE_ASSIGN_EXCEPTION = 500,
    ROLE_ALREADY_EXISTS,
    ROLE_CREATE_EXCEPTION,

    CREATE_USER = 600,
    CREATE_USER_EXCEPTION,
    USER_LOGIN_EXCEPTION,
    USER_ALREADY_EXISTS,
    USER_RETRIEVE_EXCEPTION,
    USER_CHANGE_PASSWORD,
    USER_CHANGE_PASSWORD_EXCEPTION,
}