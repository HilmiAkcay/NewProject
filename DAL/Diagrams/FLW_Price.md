```mermaid
flowchart LR
    A[Product Cost<br/>CostPrice] --> B[Pricing Rules<br/>Margin / Contract Rules]
    B --> C[Base Sales Price<br/>Cost + Margin]
    C --> D[Promotion Engine<br/>Discounts / Free Items]
    D --> E[Final Sales Price]
    E --> F[Tax Engine<br/>VAT / Tax Rules]
    F --> G[Payable Price]

    %% Styling and Annotations
    classDef pricing fill:#e3f2fd,stroke:#1e88e5,stroke-width:1px
    classDef promo fill:#fff3e0,stroke:#fb8c00,stroke-width:1px
    classDef tax fill:#e8f5e9,stroke:#43a047,stroke-width:1px

    class B pricing
    class D promo
    class F tax
