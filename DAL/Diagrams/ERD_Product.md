# Product ERD
```mermaid
erDiagram
    PRODUCT {
        int Id PK
        string Name
        int ProductGroupId
        string SKU
    }

    PRODUCTUNIT {
        int Id PK
        int ProductId
        int UnitId
        int BaseUnitId
        int Multiplier
        bool IsDefault
    }

    UNIT { 
        int Id PK 
        string Code 
        string Name 
    }

    PRODUCTGROUP { 
        int Id PK 
        string Name 
    }

    TAXRATE { 
        int Id PK 
        string Code 
        string Name 
        decimal Rate 
        ValidFrom DateTime 
        DateTime ValidTo "Nullable"
   }

    PURCHASEPRICE {
        int Id PK
        int ProductUnitId
        int AccountId
        decimal Price
        int CurrencyId
        DateTime ValidFrom
        DateTime ValidTo "Nullable"
        int TaxRateId
        bool IsGrossPrice
    }

    SALESPRICE {
        int Id PK
        int ProductUnitId
        decimal Price
        int CurrencyId
        DateTime ValidFrom
        DateTime ValidTo "Nullable"
        int TaxRateId
        bool IsGrossPrice
    }

    PRODUCTBARCODE{
        int Id PK
        int ProductUnitId
        string Barcode
        bool IsDefault
    }

    PRODUCTGROUP ||--|{ PRODUCT : "ProductGroupId"
    PRODUCTUNIT ||--|{ PRODUCTBARCODE : "ProductUnitId"
    
    PRODUCT ||--|{ PRODUCTUNIT : "ProductId"

    UNIT ||--|{ PRODUCTUNIT : "UnitId"
    UNIT ||--|{ PRODUCTUNIT : "BaseUnitId"

    TAXRATE ||--|{ SALESPRICE : "TaxRateId"
    TAXRATE ||--|{ PURCHASEPRICE : "TaxRateId"


    PRODUCTUNIT ||--|{ PURCHASEPRICE : "ProductUnitId"
    PRODUCTUNIT ||--|{ SALESPRICE : "ProductUnitId"
    
