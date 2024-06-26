﻿using Black.Model.Authentication;

namespace Black.Model.User;

public sealed record UserModel
{
    public long Id { get; init; }

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public string Email { get; init; }

    public AuthModel Auth { get; init; }
}

