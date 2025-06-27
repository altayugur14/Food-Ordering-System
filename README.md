# Yemek Sipariş Sistemi (Food Ordering System)

This project is a full-stack web application for a food ordering system, developed as part of my internship. It allows users to browse restaurants, view menus, place orders, and leave reviews. Restaurant owners can manage their restaurant details, menus, and orders.

This project was developed during my internship at **Uyumsoft** (July 2023 - August 2023).

## Features

*   **User Management:**
    *   Separate registration and login for Customers and Restaurant Owners.
    *   Role-based access control.
*   **Restaurant Platform:**
    *   Browse restaurants.
    *   View restaurant details, including address, phone number, and ratings.
    *   Search for restaurants.
*   **Ordering System:**
    *   View restaurant menus.
    *   Add items to a shopping cart.
    *   Place orders.
    *   View order history.
*   **Restaurant Management (for owners):**
    *   Manage restaurant information.
    *   Create and update menu items.
    *   View and manage incoming orders.
*   **Reviews and Ratings:**
    *   Customers can leave reviews and ratings for restaurants after an order.
*   **Promotions:**
    *   Restaurants can create and manage promotions with discount codes.

## Technologies Used

*   **Backend:**
    *   ASP.NET Core 6 MVC
    *   Entity Framework Core 7
    *   C#
*   **Database:**
    *   Microsoft SQL Server
*   **Frontend:**
    *   HTML, CSS, JavaScript
    *   Bootstrap
    *   jQuery
*   **Authentication:**
    *   ASP.NET Core Cookie Authentication

## Setup and Installation

Follow these steps to get the project running on your local machine.

### 1. Prerequisites

*   [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
*   [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (e.g., Express or Developer edition) or LocalDB.

### 2. Clone the repository

### 3. Database Setup

1.  **Create the database:**
    *   Open SQL Server Management Studio (SSMS) or your preferred SQL Server tool.
    *   Execute the script `createDatabase.sql` located in the root of the project to create the database schema and tables. This will create a database named `YemekSiparisSistemi`.

2.  **Configure the connection string:**
    *   Open the `appsettings.json` file.
    *   Locate the `ConnectionStrings` section.
    *   Modify the `YemekSiparisSistemiDatabase` connection string to point to your local SQL Server instance. The existing connection string is configured for LocalDB. If you are using SQL Server Express, for example, it might look like this:

    ```json
    "ConnectionStrings": {
      "YemekSiparisSistemiDatabase": "Server=.\\SQLEXPRESS;Database=YemekSiparisSistemi;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

    **Important:** It is recommended to use the .NET `secrets.json` feature for sensitive data like connection strings in a real-world scenario to avoid committing them to source control. For this project, you can edit it directly, but be aware of this best practice.

### 4. Run the Application

*   You can run the application using your IDE (like Visual Studio) by opening the `.sln` file and running it.
*   Alternatively, you can run it from the command line from within the repository directory:

```bash
dotnet run
```

The application should now be running and accessible at `https://localhost:XXXX` and `http://localhost:YYYY` (the ports are configured in `Properties/launchSettings.json`).

---

This project is for demonstration purposes for my portfolio.

---

# Türkçe Açıklama

Bu proje, stajım sırasında geliştirdiğim full stack bir yemek sipariş sistemi web uygulamasıdır. Kullanıcıların restoranları keşfetmesine, menüleri görüntülemesine, sipariş vermesine ve değerlendirme yapmasına olanak tanır. Restoran sahipleri ise restoran bilgilerini, menülerini ve siparişlerini yönetebilir.

Bu proje **Uyumsoft** şirketindeki stajım sırasında geliştirilmiştir (Temmuz 2023 - Ağustos 2023).

## Özellikler

*   **Kullanıcı Yönetimi:**
    *   Müşteriler ve Restoran Sahipleri için ayrı kayıt ve giriş sistemi.
    *   Rol tabanlı erişim kontrolü.
*   **Restoran Platformu:**
    *   Restoranları keşfetme.
    *   Adres, telefon numarası ve puanlar dahil restoran detaylarını görüntüleme.
    *   Restoran arama.
*   **Sipariş Sistemi:**
    *   Restoran menülerini görüntüleme.
    *   Ürünleri sepete ekleme.
    *   Sipariş verme.
    *   Sipariş geçmişini görüntüleme.
*   **Restoran Yönetimi (sahipler için):**
    *   Restoran bilgilerini yönetme.
    *   Menü öğelerini oluşturma ve güncelleme.
    *   Gelen siparişleri görüntüleme ve yönetme.
*   **Değerlendirmeler ve Puanlar:**
    *   Müşteriler sipariş sonrası restoranlar için değerlendirme ve puan bırakabilir.
*   **Promosyonlar:**
    *   Restoranlar indirim kodları ile promosyon oluşturabilir ve yönetebilir.

## Kullanılan Teknolojiler

*   **Backend:**
    *   ASP.NET Core 6 MVC
    *   Entity Framework Core 7
    *   C#
*   **Veritabanı:**
    *   Microsoft SQL Server
*   **Frontend:**
    *   HTML, CSS, JavaScript
    *   Bootstrap
    *   jQuery
*   **Kimlik Doğrulama:**
    *   ASP.NET Core Cookie Authentication

## Kurulum ve Çalıştırma

Projeyi yerel makinenizde çalıştırmak için aşağıdaki adımları izleyin.

### 1. Ön Gereksinimler

*   [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)
*   [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (örneğin Express veya Developer sürümü) veya LocalDB.

### 2. Repository'yi klonlayın


### 3. Veritabanı Kurulumu

1.  **Veritabanını oluşturun:**
    *   SQL Server Management Studio (SSMS) veya tercih ettiğiniz SQL Server aracını açın.
    *   Proje kök dizininde bulunan `createDatabase.sql` scriptini çalıştırarak veritabanı şemasını ve tablolarını oluşturun. Bu, `YemekSiparisSistemi` adında bir veritabanı oluşturacaktır.

2.  **Bağlantı dizesini yapılandırın:**
    *   `appsettings.json` dosyasını açın.
    *   `ConnectionStrings` bölümünü bulun.
    *   `YemekSiparisSistemiDatabase` bağlantı dizesini yerel SQL Server örneğinizi işaret edecek şekilde değiştirin. Mevcut bağlantı dizesi LocalDB için yapılandırılmıştır. Örneğin SQL Server Express kullanıyorsanız, şu şekilde görünebilir:

    ```json
    "ConnectionStrings": {
      "YemekSiparisSistemiDatabase": "Server=.\\SQLEXPRESS;Database=YemekSiparisSistemi;Trusted_Connection=True;TrustServerCertificate=True;"
    }
    ```

    **Önemli:** Gerçek dünya senaryolarında bağlantı dizeleri gibi hassas veriler için .NET `secrets.json` özelliğinin kullanılması, bunları kaynak kontrole gönderilmesini önlemek için önerilir. Bu proje için doğrudan düzenleyebilirsiniz, ancak bu en iyi uygulamayı unutmayın.

### 4. Uygulamayı Çalıştırın

*   Uygulamayı IDE'niz (Visual Studio gibi) kullanarak `.sln` dosyasını açıp çalıştırabilirsiniz.
*   Alternatif olarak, proje dizini içerisinden komut satırından çalıştırabilirsiniz:

```bash
dotnet run
```

Uygulama artık çalışıyor olmalı ve `https://localhost:XXXX` ve `http://localhost:YYYY` adreslerinden erişilebilir olmalıdır (portlar `Properties/launchSettings.json` dosyasında yapılandırılmıştır).

---

Bu proje portföy amaçlıdır.
