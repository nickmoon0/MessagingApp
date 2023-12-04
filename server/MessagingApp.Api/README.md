# API Endpoints Documentation

This document provides an overview of the API endpoints available in the application.

## AuthController

Responsible for user authentication and registration.

- **POST /Auth/Register**
    - **Description**: Registers a new user.
    - **Body**: `CreateUserRequest` object.
    - **Response**: User creation status.

- **POST /Auth/Authenticate**
    - **Description**: Authenticates a user.
    - **Body**: `AuthenticateUserRequest` object.
    - **Response**: Authentication status and token.

## FriendRequestController

Manages friend requests.

- **GET /FriendRequest**
    - **Description**: Retrieves pending friend requests for the logged-in user.
    - **Authorization**: Required.
    - **Response**: List of pending friend requests.

- **PUT /FriendRequest/accept/{friendRequestId}**
    - **Description**: Accepts a friend request.
    - **Authorization**: Required.
    - **Parameter**: `friendRequestId` (GUID).
    - **Response**: Status of the friend request acceptance.

## MessageController

Handles message retrieval.

- **GET /Message/{messageId}**
    - **Description**: Retrieves a specific message by its ID.
    - **Authorization**: Required.
    - **Parameter**: `messageId` (GUID).
    - **Response**: Message details.

## UserController

Manages user-related actions.

- **GET /User**
    - **Description**: Retrieves user information by ID or username.
    - **Authorization**: Required.
    - **Parameters**: `uid` (GUID, optional), `username` (string, optional).
    - **Response**: User details.

- **POST /User/{toUserId}/add**
    - **Description**: Sends a friend request to another user.
    - **Authorization**: Required.
    - **Parameter**: `toUserId` (GUID).
    - **Response**: Status of the friend request.

- **GET /User/{conversationUserId}/messages**
    - **Description**: Retrieves conversation with another user.
    - **Authorization**: Required.
    - **Parameter**: `conversationUserId` (GUID).
    - **Response**: Conversation messages.

- **POST /User/{receivingUserId}/message**
    - **Description**: Sends a message to another user.
    - **Authorization**: Required.
    - **Body**: `SendMessageRequest` object.
    - **Parameter**: `receivingUserId` (GUID).
    - **Response**: Message sending status.
