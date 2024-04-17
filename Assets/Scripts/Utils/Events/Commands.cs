using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command<T> : Query<T, CommandResponse> { }
public class Command<T, U> : Query<(T, U), CommandResponse> { }
public class Command<T, U, V> : Query<(T, U, V), CommandResponse> { }

public sealed class CommandResponse
{
    private CommandResponse() { }
    private readonly static CommandResponse ok = new();
    private readonly static CommandResponse badRequest = new();
    private readonly static CommandResponse serverError = new();
    public static CommandResponse Ok => ok;
    public static CommandResponse BadRequest => badRequest;
    public static CommandResponse ServerError => serverError;
}