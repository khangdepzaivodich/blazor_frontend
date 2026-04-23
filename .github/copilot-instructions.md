# Copilot Instructions

## General Guidelines
- First general instruction
- Second general instruction
- In chat flows, a customer (guest or logged-in) will typically connect to a single staff member/customer service representative rather than multiple.

## Code Style
- Use specific formatting rules
- Follow naming conventions

## Authentication State Management
- AuthState should store non-nullable public properties: 
  - `Token` (string)
  - `UserId` (Guid)
  - `Email` (string)
  - `Role` (string)
  - `HoTen` (string)
- Provide `SetUser(string token, Guid userId, string email, string role, string hoTen)` to update state, along with a backward-compatible overload `SetUser(string token, Guid userId, string hoTen)`.
- `Clear()` should reset all fields, raise `AuthStateChanged` notification, and maintain an `IsAuthenticated` check based on whether `Token` is not empty. 
- AuthState should notify subscribers on changes to its properties.

## UI/UX Guidelines
- Product card action icons should be hidden by default and appear only on hover, implemented using Tailwind's group and group-hover classes.