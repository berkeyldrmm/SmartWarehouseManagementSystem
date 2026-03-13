# ÇALIŞMA RAPORU

## 1) Ne yapıldığının kısa özeti
- Ürün yönetimi akışı `Admin` ve `User` yetkilerine göre ayrıştırıldı.
- `User` tarafında company-scope zorunlu hale getirildi (kullanıcı sadece kendi şirket verileri üzerinde işlem yapabiliyor).
- `Product` için request/response/filter DTO yapısı düzenlendi, mapping ve selector yaklaşımı uygulandı.
- Soft delete altyapısı güçlendirildi (`IsDeleted` + `DeletedAt`).
- `Auth` akışı geliştirildi (`login`, `user-register`), rol ataması ve company doğrulaması eklendi.
- `Company` ve `Warehouse` controller/service/repository katmanları eklendi.
- `UnitOfWork` yapısı kurularak repository katmanında `SaveChanges` çağrıları merkezi hale getirildi.
- Frontend entegrasyonu için API kullanım dokümanı oluşturuldu (`docs/frontend-api-guide.md`).

## 2) Kullanılan teknolojiler ve versiyonları
- `.NET 9`
- `C# 13.0`
- `ASP.NET Core Web API`
- `Entity Framework Core 9.0.14`
  - `Microsoft.EntityFrameworkCore`
  - `Microsoft.EntityFrameworkCore.SqlServer`
  - `Microsoft.EntityFrameworkCore.Design`
  - `Microsoft.EntityFrameworkCore.Tools`
- `ASP.NET Core Identity`
- `JWT Authentication`
  - `Microsoft.AspNetCore.Authentication.JwtBearer 9.0.14`
- `FluentValidation 12.1.1`
- `Swagger / OpenAPI` (`Swashbuckle + Microsoft.AspNetCore.OpenApi`)

## 3) Karşılaşılan sorunlar ve çözüm yolları
- **Sorun:** EF Core `PendingModelChangesWarning` (model her build’de değişiyor).
  - **Neden:** Seed sırasında dinamik password hash üretimi.
  - **Çözüm:** `HasData` için sabit `PasswordHash` kullanıldı.

- **Sorun:** User tarafında companyId’nin request ile manipüle edilebilir olması.
  - **Çözüm:** User create akışında `CreateProductForUserDto` ile `CompanyId` request’ten kaldırıldı; company bilgisi `userId` üzerinden servis katmanında çözümlendi.

- **Sorun:** Soft delete davranışının tutarsızlığı.
  - **Çözüm:** `IEntity` genişletilerek `DeletedAt` eklendi; repository delete işlemleri `IsDeleted=true` + `DeletedAt=UtcNow` olacak şekilde standardize edildi.


## 4) Mimari kararlar ve nedenleri
- **N Katmanlı mimari (Controller → Manage → Repository → Entity):**
  - İş kuralları manage katmanında, veri erişimi repository katmanında tutuldu.
- **DTO tabanlı input/output:**
  - Entity’nin doğrudan API yüzeyine çıkması engellendi; güvenlik ve bakım kolaylığı sağlandı.
- **Role + company scope:**
  - `Admin` global erişim, `User` şirket kısıtlı erişim modeli ile multi-tenant kullanım güvence altına alındı.
- **UnitOfWork:**
  - Save operasyonları merkezileştirildi; transaction kontrolü için altyapı kuruldu.
- **FluentValidation:**
  - Request doğrulamaları standartlaştırıldı.
- **Exception Handler Middleware:**	
  - Hata yönetimi middleware ile tek formatta döndürülür hale getirildi.
- **Helper sınıfı (`ProductServiceHelper`):**
  - Tekrarlayan predicate ve userId doğrulama kodları merkezi hale getirilerek servis temizliği sağlandı.

## 5) Yapay zeka kullanıldıysa hangi aşamalarda kullanıldığı
Bu çalışmada yapay zeka aşağıdaki aşamalarda kullanıldı:
- Controller ve manage katmanındaki iş yüklerinin yazılması
- Hata/derleme çıktılarının yorumlanması ve düzeltme adımlarının uygulanması
- Frontend entegrasyonuna yönelik API dokümantasyonunun hazırlanması
