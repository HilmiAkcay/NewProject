# Stock ERD
```mermaid
erDiagram
WAREHOUSE {
        int Id PK
        string Code
        string Name
        string Type "Central | Shop | Virtual"
    }

STOCKRESERVATION {
        int Id PK
        int ProductVariantId FK
        int WarehouseId FK
        decimal Quantity
        string Status "Active | Released | Consumed"
        string SourceType "Order | ServiceRequest"
        int SourceId
        int SourceLineId
    }

 STOCKFORM {
        int Id PK
        string FormType "Transfer | Purchase | Fulfillment | Return | Adjustment | Manual"
        int FromWarehouseId FK "Nullable"
        int ToWarehouseId FK "Nullable"
        string Status "Draft | Approved | Completed | Cancelled"
    }

STOCKFORMLINE {
        int Id PK
        int StockFormId FK
        int ProductVariantId FK
        int ProductUnitId FK
        decimal Quantity
    }

STOCKMOVEMENT {
        int Id PK
        int ProductVariantId FK
        int WarehouseId FK
        decimal Quantity "+/-"
        string MovementType "Purchase | Fulfillment | TransferIn | TransferOut | Adjustment | Return"
        string SourceType "StockForm | Manual | ServiceRequest"
        int SourceId
        int SourceLineId
    }

WAREHOUSE ||--o{ STOCKRESERVATION : "has"
   
    WAREHOUSE ||--o{ STOCKMOVEMENT : "movement in"
   
    STOCKFORM ||--o{ STOCKFORMLINE : "has"
    STOCKFORMLINE ||--o{ STOCKMOVEMENT : "generates"
    WAREHOUSE ||--o{ STOCKFORM : "from warehouse"
    WAREHOUSE ||--o{ STOCKFORM : "to warehouse"
   

