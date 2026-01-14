using ECommerceApp.Domain.Entities;
using ECommerceApp.Infrastructure.Data;

namespace ECommerceApp.Web.Data;

public static class NewProductSeeder
{
    public static async Task SeedNewProductsAsync(ApplicationDbContext dbContext, List<string> sellerIds)
    {
        if (sellerIds.Count == 0) return;

        var products = new List<Product>();
        var random = new Random(123);
        string RandomSeller() => sellerIds[random.Next(sellerIds.Count)];

        // ELECTRONICS - CategoryId = 1
        var electronics = new (string Name, string Desc, decimal Price)[]
        {
            ("Xiaomi Redmi Note 13 Pro", "Smartphone kamera 200MP, layar AMOLED 120Hz, baterai 5000mAh", 3999000m),
            ("Realme GT 5 Pro", "Gaming phone Snapdragon 8 Gen 3, charging 100W", 8999000m),
            ("OPPO Find X7 Ultra", "Flagship kamera Hasselblad, periscope lens", 15999000m),
            ("Vivo X100 Pro", "Smartphone kamera ZEISS dan Dimensity 9300", 12999000m),
            ("Infinix Zero 30 5G", "Smartphone 5G kamera 108MP fast charging 68W", 2999000m),
            ("TCL 50 inch 4K QLED TV", "Smart TV QLED Google TV HDR10+ Dolby Vision", 5999000m),
            ("Polytron LED 32 inch TV", "Smart TV Android HD Chromecast", 2199000m),
            ("Anker Soundcore Liberty 4", "TWS earbuds LDAC ANC spatial audio", 1499000m),
            ("Baseus 65W GaN Charger", "Charger GaN 3 port fast charging", 399000m),
            ("Ugreen USB-C Hub 10in1", "Hub USB-C HDMI 4K card reader ethernet", 699000m),
            ("TP-Link Archer AX73", "Router WiFi 6 speed 5400Mbps", 1599000m),
            ("Xiaomi Robot Vacuum S10", "Robot vacuum mopping LiDAR navigation", 3499000m),
            ("Asus ROG Ally Handheld", "PC gaming portable AMD Z1 Extreme", 10999000m),
            ("Lenovo Tab P12 Pro", "Tablet Android OLED Snapdragon 870", 8999000m),
            ("Huawei MatePad 11.5", "Tablet HarmonyOS stylus keyboard", 5499000m),
            ("DJI Mini 4 Pro Drone", "Drone video 4K HDR obstacle avoidance", 11999000m),
            ("Insta360 X4 Camera", "Kamera 360 video 8K AI editing waterproof", 7999000m),
            ("Marshall Major IV", "Headphone wireless 80 jam baterai", 1899000m),
            ("Harman Kardon Onyx 8", "Speaker bluetooth premium Hi-Fi", 4499000m),
            ("Amazfit GTR 4", "Smartwatch AMOLED dual-band GPS 150 sport", 2999000m),
        };
        foreach (var p in electronics) products.Add(CreateProduct(p.Name, p.Desc, p.Price, 1, RandomSeller(), random));

        // CLOTHING - CategoryId = 2
        var clothing = new (string Name, string Desc, decimal Price)[]
        {
            ("Batik Keris Solo Premium", "Batik tulis Solo motif parang pewarna alami", 899000m),
            ("Kemeja Batik Cap Jogja", "Kemeja batik cap motif kawung katun premium", 399000m),
            ("Sarung Tenun Samarinda", "Sarung tenun asli Samarinda motif Kalimantan", 599000m),
            ("Kebaya Kutu Baru Modern", "Kebaya modern bordiran cutting slim fit", 749000m),
            ("Baju Koko Rabbani", "Koko premium bordir bahan toyobo", 349000m),
            ("Gamis Nibras Modern", "Gamis syari desain modern bahan wolfis", 299000m),
            ("Sepatu Wakai Slip-On", "Sepatu slip-on nyaman desain Jepang sol ringan", 499000m),
            ("Sandal Eiger Gunung", "Sandal outdoor sol Vibram tali adjustable", 449000m),
            ("Jaket Eiger Waterproof", "Jaket gunung waterproof membran breathable", 899000m),
            ("Celana Cargo Consina", "Celana outdoor banyak kantong quick-dry", 399000m),
            ("Tas Ransel Bodypack", "Ransel laptop organizer anti-theft", 599000m),
            ("Topi Fedora Pria", "Topi fedora bahan felt premium gaya kasual", 199000m),
            ("Kacamata Sunglass Polarized", "Kacamata hitam lensa polarized frame ringan", 299000m),
            ("Dompet Kulit Asli Garut", "Dompet pria kulit asli Garut jahitan tangan", 349000m),
            ("Ikat Pinggang Kulit Sapi", "Belt kulit asli buckle stainless steel", 249000m),
            ("Kaos Polo Crocodile", "Polo shirt premium bahan cotton pique", 299000m),
            ("Celana Jeans Edwin Tokyo", "Jeans regular fit selvedge denim", 799000m),
            ("Dress Midi Floral", "Dress midi print floral bahan viscose", 449000m),
            ("Blouse Atasan Wanita", "Blouse kerja detail ruffle bahan crepe", 349000m),
            ("Rok Plisket Modern", "Rok plisket bahan premium warna elegan", 299000m),
        };
        foreach (var p in clothing) products.Add(CreateProduct(p.Name, p.Desc, p.Price, 2, RandomSeller(), random));

        // BOOKS - CategoryId = 3
        var books = new (string Name, string Desc, decimal Price)[]
        {
            ("Laskar Pelangi - Andrea Hirata", "Novel inspiratif perjuangan anak Belitung", 89000m),
            ("Bumi Manusia - Pramoedya", "Karya sastra klasik era kolonial", 119000m),
            ("Filosofi Teras - Henry M", "Buku filosofi Stoa kehidupan modern", 99000m),
            ("Laut Bercerita - Leila", "Novel aktivis era Orde Baru", 109000m),
            ("Pulang - Tere Liye", "Novel keluarga petualangan Indonesia", 89000m),
            ("Seni Bersikap Bodo Amat", "Buku self-help fokus hal penting", 89000m),
            ("Atomic Habits Indonesia", "Edisi Indonesia bestseller James Clear", 119000m),
            ("Intelligent Investor ID", "Panduan value investing Indonesia", 149000m),
            ("Belajar Python Pemula", "Panduan programming Python praktis", 129000m),
            ("Digital Marketing 101", "Panduan pemasaran digital bisnis", 99000m),
            ("Resep Masakan Nusantara", "200 resep masakan tradisional Indonesia", 149000m),
            ("Parenting Zaman Now", "Panduan mengasuh anak era digital", 89000m),
            ("Mindfulness Pemula", "Panduan meditasi mindfulness praktis", 79000m),
            ("Bahasa Inggris Otodidak", "Belajar bahasa Inggris mandiri efektif", 99000m),
            ("Investasi Saham Pemula", "Panduan investasi saham lengkap", 109000m),
            ("Startup Indonesia", "Kisah sukses startup unicorn Indonesia", 129000m),
            ("Kamus Besar Bahasa Indonesia", "KBBI edisi lengkap update kata baru", 199000m),
            ("Al-Quran Terjemah Perkata", "Al-Quran terjemahan per kata tafsir", 179000m),
            ("Ensiklopedia Anak Cerdas", "Ensiklopedia anak ilustrasi menarik", 249000m),
            ("Komik Si Juki", "Komik humor Indonesia menghibur", 69000m),
        };
        foreach (var p in books) products.Add(CreateProduct(p.Name, p.Desc, p.Price, 3, RandomSeller(), random));

        // HOME AND GARDEN - CategoryId = 4
        var homeGarden = new (string Name, string Desc, decimal Price)[]
        {
            ("Kompor Gas Rinnai 2 Tungku", "Kompor gas anti-rust pengapian otomatis", 699000m),
            ("Rice Cooker Cosmos 1.8L", "Magic com anti lengket fungsi warm", 399000m),
            ("Blender Philips HR2157", "Blender ProBlend pisau titanium", 599000m),
            ("Setrika Uap Panasonic", "Setrika uap powerful anti-drip", 449000m),
            ("Kipas Angin Maspion 16", "Kipas berdiri 3 kecepatan remote", 399000m),
            ("Dispenser Miyako Hot Cold", "Dispenser kompresor galon bawah", 899000m),
            ("Lemari Plastik Olymplast", "Lemari plastik kokoh 4 laci", 549000m),
            ("Kasur Busa Royal Foam", "Kasur busa kepadatan tinggi quilting", 1499000m),
            ("Meja Makan Minimalis", "Set meja makan kayu jati 4 kursi", 2499000m),
            ("Sofa Bed Lipat", "Sofa dilipat jadi tempat tidur", 1999000m),
            ("Rak Buku Kayu 5 Tingkat", "Rak buku kayu pinus natural", 799000m),
            ("Karpet Turki 200x300", "Karpet import motif oriental tebal", 1299000m),
            ("Gorden Blackout 150x250", "Gorden anti sinar matahari hook ring", 299000m),
            ("Pot Tanaman Keramik Set", "Set pot keramik desain minimalis", 249000m),
            ("Tanaman Monstera", "Tanaman hias indoor daun unik", 199000m),
            ("Lampu LED Philips 12W", "Set lampu LED hemat energi kuning", 129000m),
            ("Kunci Pintu Digital", "Smart lock fingerprint password", 3499000m),
            ("CCTV Xiaomi 1080p", "Kamera CCTV night vision two-way audio", 399000m),
            ("Panci Set Teflon 5pcs", "Set panci anti lengket tutup kaca", 599000m),
            ("Vacuum Cleaner Deerma", "Vacuum handheld suction powerful", 899000m),
        };
        foreach (var p in homeGarden) products.Add(CreateProduct(p.Name, p.Desc, p.Price, 4, RandomSeller(), random));

        // SPORTS - CategoryId = 5
        var sports = new (string Name, string Desc, decimal Price)[]
        {
            ("Sepeda MTB Polygon Premier", "MTB entry level frame alloy 21 speed", 3999000m),
            ("Sepeda Lipat Pacific Noris", "Sepeda lipat 20 inch 7 speed chromoly", 2999000m),
            ("Raket Badminton Li-Ning", "Raket badminton pro TB Nano", 1999000m),
            ("Shuttlecock RSL Classic", "Kok bulu angsa grade A 1 tube isi 12", 179000m),
            ("Bola Basket Molten GG7X", "Bola basket official kulit PU premium", 699000m),
            ("Bola Sepak Adidas UCL", "Bola sepak official match ball thermal", 1499000m),
            ("Jersey Timnas Indonesia", "Jersey timnas terbaru Dri-Fit", 449000m),
            ("Sepatu Futsal Specs", "Sepatu futsal sol phylon upper mesh", 599000m),
            ("Treadmill Kettler Marathon", "Treadmill motor 2HP incline elektronis", 8999000m),
            ("Dumbell Set 20kg Kettler", "Set dumbell cast iron bar chrome", 1299000m),
            ("Matras Yoga 8mm NBR", "Matras yoga tebal anti slip", 249000m),
            ("Resistance Band Set 5pcs", "Set resistance band 5 level resistensi", 149000m),
            ("Sarung Tinju Everlast", "Sarung tinju 12oz padding busa premium", 599000m),
            ("Tenda Camping Eiger 4P", "Tenda dome 4 orang waterproof", 1299000m),
            ("Sleeping Bag Consina", "Sleeping bag suhu comfort -5C", 599000m),
            ("Sepatu Hiking Eiger", "Sepatu hiking waterproof Vibram", 1499000m),
            ("Tas Carrier Rei 60L", "Carrier frame internal rain cover", 899000m),
            ("Jam G-Shock GA-2100", "Jam outdoor 200m water resist", 1699000m),
            ("Kacamata Renang Speedo", "Kacamata renang anti-fog UV protection", 299000m),
            ("Helm Sepeda Polygon", "Helm sepeda EPS foam visor", 449000m),
        };
        foreach (var p in sports) products.Add(CreateProduct(p.Name, p.Desc, p.Price, 5, RandomSeller(), random));

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();
    }

    private static Product CreateProduct(string name, string desc, decimal price, int categoryId, string sellerId, Random random)
    {
        var colors = new Dictionary<int, string>
        {
            { 1, "2563eb" }, { 2, "dc2626" }, { 3, "16a34a" }, { 4, "ca8a04" }, { 5, "9333ea" }
        };
        var color = colors.GetValueOrDefault(categoryId, "333333");
        var firstName = name.Split(' ')[0];
        
        return new Product
        {
            Name = name,
            Description = desc,
            Price = price,
            Stock = random.Next(10, 200),
            ImageUrl = $"https://placehold.co/400x400/{color}/fff?text={Uri.EscapeDataString(firstName)}",
            SellerId = sellerId,
            CategoryId = categoryId,
            IsActive = true,
            CreatedAt = DateTime.Now.AddDays(-random.Next(1, 90))
        };
    }
}
