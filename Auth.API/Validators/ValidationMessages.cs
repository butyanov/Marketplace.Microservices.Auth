namespace Auth.API.Validators;

public static class ValidationMessages
{
    public const string EmptyName = "Name field cannot be empty";
    public const string TooShortName = "Name is too short";
    public const string TooLongName = "Name is too long";
    public const string NameContainsWrongSymbols = "Name contains imadmissible symbols";

    public const string EmptyEmail = "Email field cannot be empty";
    public const string IncorrectEmail = "Email is incorrect";
    public const string EmailAlreadyExists = "Email is already used";

    public const string EmptyPassword = "Password cannot be empty";
    public const string PasswordContainsWrongSymbols = "Password contains inadmissible symbols";
    public const string TooShortPassword = "Password is too short";
    public const string TooLongPassword = "Password is too long";

    public const string TooShortPhone = "Number is too short";
    public const string TooLongPhone = "Number is too long";
    public const string IncorrectPhone = "Number has incorrect format";
    
}