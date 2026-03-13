🏭 **SmartWarehouseManagementSystem (SWMS)**

SmartWarehouseManagementSystem, işletmelerin envanter hareketlerini, depo doluluk oranlarını ve sevkiyat süreçlerini yönetebilmeleri için geliştirilmiş, yüksek performanslı bir Kurumsal Depo Yönetim Sistemi çözümüdür.

Bu proje; veri bütünlüğü, ölçeklenebilir mimari ve temiz kod prensiplerine odaklanılarak bir Case Study olarak inşa edilmiştir.

🌟 **Temel Yetenekler**

Anlık Stok Takibi: Ürün giriş ve çıkışlarının gerçek zamanlı veritabanı yansıması.

Multi-Tenant Desteği: Tek bir veritabanı üzerinde izole edilmiş çoklu depo/müşteri yönetimi.

Gelişmiş Filtreleme: Ürün tipi, depo lokasyonu ve kritik stok seviyelerine göre dinamik sorgulama.

🛠️ **Teknik Altyapı**

Framework: .NET 9 (Web API)

ORM: Entity Framework Core (SQL Server)

Mimari: N-Tier Architecture (Katmanlı Mimari)

Dokümantasyon: Swagger

🏗️ **Mimari Katmanlar**

Proje, sorumlulukların ayrılması (Separation of Concerns) prensibiyle 4 ana katmandan oluşur:

Controller Layer: Controller yapıları ve Global Exception Handling süreçlerini barındırır.

Manager Layer: İş kurallarının (Validation, Calculation) ve servis mantığının bulunduğu katmandır.

Repository Layer: DbContext, Migrations ve veritabanı erişim paternlerini (Repository) içerir.

Entity Layer: Veritabanı tablolarını temsil eden modeller ve veri taşıma nesnelerini (DTO) barındırır.
