namespace P4P.Exceptions;

public class InvalidEmailVerificationTokenException : HttpException
{
    public InvalidEmailVerificationTokenException()
        : base(
            StatusCodes.Status422UnprocessableEntity,
            "Bloga patvirtinimo nuoroda, bandykite dar kartą"
        )
    {
    }
}
