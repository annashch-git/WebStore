-- Назначаем перевозчика (например, DHL, id = 1) и отмечаем как "Shipped"
UPDATE orders
SET "CarrierId"      = 1,
    tracking_number = 'DH123456789',
    shipped_date    = NOW(),
    order_status    = 'Shipped'
WHERE order_id = 1;

-- Проверка обновлённого заказа
SELECT o.order_id,
       o.order_status,
       c.carrier_name,
       o.tracking_number,
       o.shipped_date,
       o.delivered_date
FROM orders o
LEFT JOIN carriers c ON o."CarrierId" = c."CarrierId"
WHERE o.order_id = 1;

-- Позже: отмечаем заказ как доставленный
UPDATE orders
SET delivered_date = NOW(),
    order_status   = 'Delivered'
WHERE order_id = 1;

-- Повторная проверка
SELECT o.order_id,
       o.order_status,
       c.carrier_name,
       o.tracking_number,
       o.shipped_date,
       o.delivered_date
FROM orders o
LEFT JOIN carriers c ON o."CarrierId" = c."CarrierId"
WHERE o.order_id = 1;
