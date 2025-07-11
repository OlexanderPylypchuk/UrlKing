# UrlKing 

UrlKing is a lightweight URL shortening service and client. It offers:

* A REST API for creating, retrieving, and deleting short URLs.
* A client library to simplify consuming the API.

---

## 🧠 Features

* **API service** – HTTP endpoints to generate and manage short URLs.
* **Client** – Easy-to-use library to integrate UrlKing into your application.

---

## 🔧 Setup

### API Server

1. Clone the repository:

   ```bash
   git clone https://github.com/OlexanderPylypchuk/UrlKing.git
   cd UrlKing
   ```
2. Install dependencies:

   ```bash
   npm install
   ```
3. Start the server:

   ```bash
   npm start
   ```

The API will be available at `https://localhost:7173`.

---

## 📦 REST API

### Endpoints

#### Create Short URL

* **POST** `/api/url`
* **Request Body**:

  ```json
  { "url": "https://example.com" }
  ```
* **Response**:

  ```json
  {
    "code": "abc123",
    "longUrl": "https://example.com",
    "shortUrl": "https://localhost:7173/shorturl/abc123",
    "createdDate": "2025-07-10T12:34:56Z",
    "userId": "user-guid-here"
  }
  ```

#### Get Original URL

* **GET** `/api/:code`
* **Response**:

  ```json
  {
    "code": "code",
    "longUrl": "https://example.com",
    "shortUrl": "https://localhost:7173/shorturl/abc123",
    "createdDate": "2025-07-10T12:34:56Z",
    "userId": "user-guid-here"
  }
  ```

#### Delete Short URL

* **DELETE** `/api/:code`
* **Response**: `200 Ok`

---

## 🚀 React Client Usage

The frontend is built with **React**, **Vite**, and **Tailwind CSS**.

### Setup

1. Navigate to the client folder:

   ```bash
   cd client
   ```
2. Install dependencies:

   ```bash
   npm install
   ```
3. Start the development server:

   ```bash
   npm run dev
   ```

##NOT IMPLEMENTED DUE TO UNIVERSITY PRACTICE

1. Info about link page
2. About page
3. Unit testing
   
