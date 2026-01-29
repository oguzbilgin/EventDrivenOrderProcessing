using EventDriven.OrderProcessing.Application.Auth.Commands.Login;
using MediatR;

namespace EventDriven.OrderProcessing.Application.Auth.Commands.Refresh;
public sealed record RefreshTokenCommand(string RefreshToken) : IRequest<LoginResponse>;
