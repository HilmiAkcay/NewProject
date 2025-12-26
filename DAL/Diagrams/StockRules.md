# Stock & Reservation Design – Critical Notes

## 1. Stock is event-based
- Stock is derived from **stock movements**, never stored as a balance.
- The system must never update a “stock = X” field.
- All physical stock changes are recorded as **append-only events**.

---

## 2. Base unit is mandatory and immutable
- Every product variant has exactly **one base unit**.
- The base unit represents the **smallest indivisible physical unit**.
- The base unit **must not change** once any of the following exist:
  - stock movements
  - reservations
  - orders or invoices
- Changing selling units does **not** mean changing the base unit.

---

## 3. Units belong to documents, not stock
- Users may enter quantities in BOX, PACK, KG, etc.
- Stock tables **must not store user-facing units**.
- Unit conversion happens **before stock movement creation**.
- Stock movements always store quantities in **base units only**.

---

## 4. Separation of responsibilities
| Layer | Responsibility |
|------|---------------|
| Order / Invoice | User intent, pricing, units |
| StockReservation | Locking base-unit stock |
| StockForm / Line | Operational document (human-readable) |
| StockMovement | Physical stock truth |

Mixing these responsibilities is not allowed.

---

## 5. StockReservation is a lock, not a document
- Reservation quantity is always in **base units**.
- StockReservation must not store product units or UI quantities.
- Reservations can be:
  - partially consumed
  - fully consumed
  - released
- Every reservation must reference its source (Order / ServiceRequest).

---

## 6. StockMovement is the single source of truth
- StockMovement is **immutable**.
- Each row represents a real physical change.
- All stock reporting is derived from this table.
- No stock movement means no stock change.

---

## 7. StockForm represents intent, not execution
- StockForm represents **planned operations**.
- Stock must change **only when a StockForm is posted/approved**.
- Draft or cancelled forms must never affect stock.

---

## 8. StockFormLine stores human-entered quantities
- Quantity on StockFormLine represents **user-entered quantity**.
- ProductUnitId is allowed on StockFormLine.
- Conversion to base units happens at posting time.
- User-entered quantities must never be overwritten by base quantities.

---

## 9. Partial fulfillment is a first-class scenario
- One StockFormLine may generate:
  - multiple stock movements
  - or none at all
- Fulfillment, cancellation, and return are **distinct business events**.

---

## 10. Cancellation does not change stock
- Cancellation releases reservations.
- Cancellation does not create stock movements.
- Returns are the only operation that increases stock after fulfillment.

---

## 11. Stock transfer is two movements
- A transfer consists of:
  - stock OUT from the source warehouse
  - stock IN to the destination warehouse
- Stock must never be moved implicitly.
- Source and destination warehouses must always be explicit.

---

## 12. Warehouses define stock scope
- Stock always belongs to **exactly one warehouse**.
- One shop may use multiple warehouses.
- Multiple shops may share a central warehouse.

---

## 13. Unit conversion is frozen at execution time
- Unit multipliers are evaluated at posting time.
- Historical stock movements must never be recalculated.
- Changing unit definitions must not affect historical data.

---

## 14. Base unit selection must be enforced
- Base unit is selectable only during product creation.
- Base unit must be chosen from a restricted, allowed list.
- Base unit becomes read-only immediately after creation.

---

## 15. Business meaning changes require a new variant
- If packaging or unit semantics change:
  - create a new product variant
  - transfer stock using a StockForm
- Historical data must not be mutated.

---

## 16. No direct stock manipulation
- Application code must never:
  - insert StockMovement directly
  - update stock balances
- All stock changes must go through StockForm posting logic.

---

## 17. Auditability over convenience
- Historical correctness is more important than UI simplicity.
- Any shortcut that breaks traceability must not be implemented.

---

## 18. Design for future complexity
The design must support:
- partial fulfillment
- partial cancellation
- recounts and adjustments
- multi-warehouse scenarios
- unit changes without historical corruption

---

## Final Principle
> **Documents speak human.  
> Stock speaks math.  
> Movements speak truth.**

   

