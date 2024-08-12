# Papara Digital Product Platform API

This project is a web API for managing a digital product platform, including functionalities for users, products, orders, categories, and coupons.

## Table of Contents

- [Getting Started](#getting-started)
- [Environment Setup](#environment-setup)
- [Postman Collection](#postman-collection)
- [API Endpoints](#api-endpoints)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

These instructions will help you set up and run the project on your local machine for development and testing purposes.

### Prerequisites

- [.NET Core SDK](https://dotnet.microsoft.com/download) version 6.0 or later.
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) or any other database you prefer.
- [Postman](https://www.postman.com/downloads/) for testing the API.

### Installation

1. **Clone the repository:**

    ```sh
    git clone https://github.com/engferhatyildiz/Papara-Final-Project
    cd papara-digital-product-platform
    ```

2. **Restore dependencies:**

    ```sh
    dotnet restore
    ```

3. **Set up the database:**

    - Update the `appsettings.json` file with your database connection string.
    - Apply migrations:

    ```sh
    dotnet ef database update
    ```

4. **Run the application:**

    ```sh
    dotnet run
    ```

5. The API will be running at `https://localhost:5001`.

## Environment Setup

Make sure you have the necessary environment variables configured for:

- `ConnectionStrings__DefaultConnection`: The connection string for your database.
- `JWT__Secret`: The secret key for JWT authentication.

## Postman Collection

To test the API endpoints, you can use the provided Postman collection.

### Import the Collection

1. Download the Postman collection from the repository or directly [here](postman_collection.json).
2. Open Postman, go to the workspace where you'd like to import the collection.
3. Click on the `Import` button in the top left corner.
4. Select the `Papara Digital Product Platform API.postman_collection.json` file you just downloaded.
5. The collection should now appear in your Postman under the `Collections` tab.

### Postman Environment

Make sure to configure the environment variables in Postman:

- `baseUrl`: Base URL of your API, typically `https://localhost:5001`.
- `token`: JWT token for authorization.

## API Endpoints

Here are some of the key endpoints available in this API:

- **Admin Endpoints**
    - `POST /api/Admin/registerAdmin`: Register a new admin.
    - `PUT /api/Admin/users/:email`: Update a user by email.
    - `DELETE /api/Admin/users/:email`: Delete a user by email.

- **User Endpoints**
    - `POST /api/Users/register`: Register a new user.
    - `POST /api/Users/login`: Login a user.

- **Order Endpoints**
    - `POST /api/Orders`: Create a new order.
    - `GET /api/Orders`: Get all orders.
    - `GET /api/Orders/active/:userId`: Get active orders for a user.
    - `GET /api/Orders/history/:userId`: Get order history for a user.

- **Product Endpoints**
    - `POST /api/Admin/products`: Add a new product.
    - `PUT /api/Admin/products/:name`: Update a product by name.
    - `DELETE /api/Admin/products/:name`: Delete a product by name.

- **Coupon Endpoints**
    - `POST /api/Admin/coupons`: Create a new coupon.
    - `GET /api/Coupons`: Get all coupons.
    - `GET /api/Coupons/:code/status`: Get the status of a coupon.

For a full list of endpoints and details, refer to the [Postman collection](postman_collection.json).

## Contributing

If you wish to contribute to this project, please fork the repository and submit a pull request.

## License

This project is licensed under the MIT License.
