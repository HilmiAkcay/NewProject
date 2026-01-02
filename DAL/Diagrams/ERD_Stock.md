# Stock ERD

- [Check Stock Process Rules](StockRules.md)
- [Check Stock Flow](StockFlow.md)
 
```mermaid
erDiagram

WAREHOUSE {
        uuid Id PK
        string Code
        string Name
        string Type "Central | Shop | Virtual"
    }

STOCKRESERVATION {
        uuid Id PK
        uuid ProductUnitId FK
        uuid WarehouseId FK
        decimal Quantity
        string Status "Active | Released"
        uuid OrderLineId FK
    }

 STOCKFORM {
        uuid Id PK
        string FormType "Transfer | Purchase | Fulfillment | Return | Adjustment | Manual"
        uuid FromWarehouseId FK "Nullable"
        uuid ToWarehouseId FK "Nullable"
        string Status "Draft | Approved | Completed | Cancelled"
    }

STOCKFORMLINE {
        uuid Id PK
        uuid StockFormId FK
        uuid ProductUnitId FK
        decimal Quantity
    }

STOCKMOVEMENT {
        uuid Id PK
        uuid ProductUnitId FK
        uuid WarehouseId FK
        decimal Quantity "+/-"
        uuid StockFormId FK
        uuid StockFormLineId FK
    }

STOCKBALANCE {
 uuid ProductUnitId PK
 uuid WarehouseId PK
 decimal PhysicalQuantity
}

WAREHOUSE ||--o{ STOCKRESERVATION : "has"
   
    WAREHOUSE ||--o{ STOCKMOVEMENT : "movement in"
   
    STOCKFORM ||--o{ STOCKFORMLINE : "has"
    STOCKFORMLINE ||--o{ STOCKMOVEMENT : "generates"
    WAREHOUSE ||--o{ STOCKFORM : "from warehouse"
    WAREHOUSE ||--o{ STOCKFORM : "to warehouse"
   

