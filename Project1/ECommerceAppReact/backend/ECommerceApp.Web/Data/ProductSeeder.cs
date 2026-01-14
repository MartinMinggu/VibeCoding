using ECommerceApp.Domain.Entities;
using ECommerceApp.Infrastructure.Data;

namespace ECommerceApp.Web.Data;

public static class ProductSeeder
{
    public static async Task SeedProductsAsync(ApplicationDbContext dbContext, string sellerId)
    {
        if (dbContext.Products.Any())
            return;

        var products = new List<Product>();
        var random = new Random(42); // Fixed seed for consistent data

        // Electronics (CategoryId = 1)
        var electronics = new[]
        {
            ("Samsung Galaxy S24 Ultra 256GB", "Smartphone flagship Samsung dengan S Pen, kamera 200MP, dan layar Dynamic AMOLED 2X 6.8 inci.", 19999000m, "https://images.samsung.com/is/image/samsung/p6pim/id/2401/gallery/id-galaxy-s24-ultra-s928-sm-s928bztqxid-thumb-539573880"),
            ("Sony WH-1000XM5 Headphone", "Headphone wireless premium dengan noise cancelling terbaik di kelasnya dan baterai 30 jam.", 5499000m, "https://www.sony.co.id/image/5d02da5df552836db894cead8a68f5f3"),
            ("iPad Pro 12.9\" M2 256GB", "Tablet Apple dengan chip M2, layar Liquid Retina XDR, dan dukungan Apple Pencil generasi ke-2.", 18999000m, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/ipad-pro-12-select-wifi-spacegray-202104?wid=400"),
            ("Samsung 55\" QLED 4K Smart TV", "TV Samsung QLED dengan teknologi Quantum Dot, Tizen OS, dan resolusi 4K UHD.", 12999000m, "https://images.samsung.com/is/image/samsung/p6pim/id/qa55q60cauxid/gallery/id-qled-q60c-qa55q60cauxid-thumb-537227413"),
            ("Logitech MX Master 3S Mouse", "Mouse wireless premium untuk produktivitas dengan scroll MagSpeed dan sensor 8000 DPI.", 1599000m, "https://resource.logitech.com/w_386,ar_1.0,c_limit,f_auto,q_auto,dpr_2.0/d_transparent.gif/content/dam/logitech/en/products/mice/mx-master-3s/gallery/mx-master-3s-mouse-top-view-graphite.png"),
            ("Apple Watch Series 9 45mm", "Smartwatch Apple dengan chip S9, layar Always-On Retina, dan fitur Double Tap.", 7499000m, "https://store.storeimages.cdn-apple.com/4982/as-images.apple.com/is/watch-s9-digitalmat-gallery-2-202309?wid=400"),
            ("Dell XPS 15 Laptop", "Laptop premium dengan Intel Core i7, 16GB RAM, 512GB SSD, dan layar OLED 3.5K.", 28999000m, "https://i.dell.com/is/image/DellContent/content/dam/ss2/product-images/dell-client-products/notebooks/xps-notebooks/xps-15-9530/media-gallery/black/notebook-xps-15-9530-t-black-gallery-1.psd"),
            ("Canon EOS R6 Mark II", "Kamera mirrorless full-frame dengan sensor 24.2MP, video 4K 60fps, dan stabilisasi 8 stop.", 42999000m, "https://asia.canon/media/image/2022/11/02/c6c16f87bf7e4bb9b6a5f25e33d1e611_EOS+R6+Mark+II+RF24-105mm+f4L+Front+Slant.png"),
            ("JBL Flip 6 Speaker Bluetooth", "Speaker portabel tahan air IP67 dengan suara bass yang powerful dan baterai 12 jam.", 1799000m, "https://www.jbl.com/dw/image/v2/BFND_PRD/on/demandware.static/-/Sites-masterCatalog_Harman/default/dwf1e5ecb7/JBL_Flip6_Front_Teal.png"),
            ("Asus ROG Phone 7 Ultimate", "Gaming phone dengan Snapdragon 8 Gen 2, layar 165Hz AMOLED, dan AeroActive Cooler.", 17999000m, "https://dlcdnwebimgs.asus.com/gain/8ed0e0e1-f1a0-4ab2-9f9e-7a7777e3b2d7/"),
            ("GoPro Hero 12 Black", "Action camera dengan video 5.3K, HyperSmooth 6.0, dan HDR foto/video.", 6999000m, "https://gopro.com/content/dam/help/hero12-black/png/HERO12-Black-images.png"),
            ("Bose QuietComfort Ultra Earbuds", "Earbuds wireless dengan noise cancelling premium dan Immersive Audio.", 4299000m, "https://assets.bose.com/content/dam/Bose_DAM/Web/consumer_electronics/global/products/headphones/qc-ultra-earbuds/product_silo_images/QCUltraEarbuds_Black_EC_Hero.png"),
            ("Nintendo Switch OLED Model", "Konsol gaming portable dengan layar OLED 7 inci dan dock dengan LAN port.", 5499000m, "https://assets.nintendo.com/image/upload/f_auto/q_auto/dpr_2.0/c_scale,w_400/ncom/en_US/switch/site-design-update/hardware-background-color"),
            ("LG 27\" UltraGear Gaming Monitor", "Monitor gaming 27 inci dengan IPS 1ms, 144Hz, dan G-Sync Compatible.", 4999000m, "https://www.lg.com/id/images/monitor/md06047556/gallery/27GN750-B-D-01.jpg"),
            ("Anker PowerCore 26800mAh", "Power bank kapasitas besar dengan output 3 port dan teknologi PowerIQ.", 799000m, "https://m.media-amazon.com/images/I/61Jo575M1TL._AC_SL1200_.jpg"),
        };

        foreach (var (name, desc, price, img) in electronics)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                Stock = random.Next(10, 100),
                ImageUrl = img,
                SellerId = sellerId,
                CategoryId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
            });
        }

        // Clothing (CategoryId = 2)
        var clothing = new[]
        {
            ("Uniqlo AIRism Cotton T-Shirt", "Kaos dengan teknologi AIRism yang sejuk dan menyerap keringat, tersedia berbagai warna.", 199000m),
            ("Levi's 501 Original Jeans", "Celana jeans klasik dengan potongan straight fit yang timeless.", 1299000m),
            ("Adidas Ultraboost 23", "Sepatu lari dengan teknologi Boost untuk kenyamanan maksimal.", 2799000m),
            ("Zara Slim Fit Blazer", "Blazer slim fit untuk tampilan formal maupun casual.", 1599000m),
            ("H&M Oversized Hoodie", "Hoodie oversized yang nyaman untuk daily wear.", 499000m),
            ("Nike Dri-FIT Running Shorts", "Celana pendek olahraga dengan teknologi Dri-FIT.", 549000m),
            ("Converse Chuck Taylor All Star", "Sneakers ikonik yang cocok untuk berbagai gaya.", 899000m),
            ("The North Face Thermoball Jacket", "Jaket dengan insulasi ThermoBall untuk kehangatan optimal.", 3299000m),
            ("Calvin Klein Cotton Boxer Briefs 3-Pack", "Set celana dalam premium dari bahan katun berkualitas.", 699000m),
            ("New Balance 574 Classic", "Sneakers retro dengan desain klasik yang stylish.", 1499000m),
            ("Polo Ralph Lauren Oxford Shirt", "Kemeja oxford dengan logo iconic Polo.", 1899000m),
            ("Puma RS-X Sneakers", "Sneakers chunky dengan desain futuristik.", 1699000m),
            ("Under Armour Tech 2.0 T-Shirt", "Kaos olahraga dengan material anti-odor.", 449000m),
            ("Vans Old Skool", "Sneakers skateboard klasik dengan side stripe.", 999000m),
            ("Uniqlo Ultra Light Down Jacket", "Jaket bulu angsa ultra ringan yang bisa dilipat.", 999000m),
        };

        foreach (var (name, desc, price) in clothing)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                Stock = random.Next(20, 150),
                ImageUrl = $"https://placehold.co/400x400/333/fff?text={Uri.EscapeDataString(name.Split(' ')[0])}",
                SellerId = sellerId,
                CategoryId = 2,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
            });
        }

        // Books (CategoryId = 3)
        var books = new[]
        {
            ("Atomic Habits - James Clear", "Buku tentang cara membangun kebiasaan baik dan menghilangkan kebiasaan buruk.", 159000m),
            ("Rich Dad Poor Dad - Robert Kiyosaki", "Buku keuangan klasik tentang mindset orang kaya.", 129000m),
            ("The Psychology of Money - Morgan Housel", "Pelajaran abadi tentang kekayaan, keserakahan, dan kebahagiaan.", 149000m),
            ("Thinking, Fast and Slow - Daniel Kahneman", "Eksplorasi mendalam tentang dua sistem berpikir manusia.", 189000m),
            ("Deep Work - Cal Newport", "Panduan untuk fokus dalam dunia yang penuh distraksi.", 139000m),
            ("The Lean Startup - Eric Ries", "Metodologi untuk membangun startup yang sukses.", 169000m),
            ("Sapiens - Yuval Noah Harari", "Sejarah singkat umat manusia dari zaman purba.", 199000m),
            ("The 7 Habits of Highly Effective People", "7 kebiasaan untuk meningkatkan efektivitas pribadi.", 159000m),
            ("Start with Why - Simon Sinek", "Mengapa pemimpin hebat menginspirasi aksi.", 149000m),
            ("Zero to One - Peter Thiel", "Catatan tentang startup dan cara membangun masa depan.", 139000m),
            ("Good to Great - Jim Collins", "Mengapa beberapa perusahaan berhasil dan yang lain tidak.", 179000m),
            ("The Power of Now - Eckhart Tolle", "Panduan menuju pencerahan spiritual.", 149000m),
            ("Dune - Frank Herbert", "Novel fiksi ilmiah klasik tentang politik dan ekologi.", 189000m),
            ("1984 - George Orwell", "Novel distopia klasik tentang pengawasan pemerintah.", 129000m),
            ("To Kill a Mockingbird - Harper Lee", "Novel klasik tentang keadilan dan rasisme di Amerika.", 139000m),
        };

        foreach (var (name, desc, price) in books)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                Stock = random.Next(30, 200),
                ImageUrl = $"https://placehold.co/400x400/1a365d/fff?text={Uri.EscapeDataString(name.Split(' ')[0])}",
                SellerId = sellerId,
                CategoryId = 3,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
            });
        }

        // Home & Garden (CategoryId = 4)
        var homeGarden = new[]
        {
            ("Philips Air Fryer XXL", "Air fryer kapasitas besar dengan teknologi Rapid Air.", 3999000m),
            ("Dyson V15 Detect Vacuum", "Vacuum cleaner cordless dengan laser dust detection.", 12999000m),
            ("IKEA MALM Bed Frame Queen", "Rangka tempat tidur minimalis dengan storage.", 3499000m),
            ("Xiaomi Air Purifier 4 Pro", "Pembersih udara dengan filter HEPA dan IoT.", 2699000m),
            ("KitchenAid Stand Mixer", "Mixer profesional untuk baking enthusiast.", 8999000m),
            ("Nespresso Vertuo Next", "Mesin kopi kapsul dengan sistem centrifusion.", 2999000m),
            ("iRobot Roomba i7+", "Robot vacuum dengan self-emptying base.", 9999000m),
            ("Philips Hue Starter Kit", "Smart lighting dengan 16 juta warna.", 2499000m),
            ("Daikin Split AC 1 PK", "AC inverter hemat energi dengan filter anti-bakteri.", 6999000m),
            ("Samsung Family Hub Refrigerator", "Kulkas pintar dengan layar sentuh dan kamera internal.", 35999000m),
            ("Tefal Ingenio Cookware Set", "Set panci dan wajan dengan pegangan lepas-pasang.", 1999000m),
            ("MUJI Aroma Diffuser", "Diffuser aromaterapi dengan LED dan timer.", 799000m),
            ("Weber Spirit E-310 Gas Grill", "Panggangan gas premium untuk BBQ.", 12499000m),
            ("Vitamix E310 Blender", "Blender profesional dengan motor 2HP.", 8999000m),
            ("Herman Miller Aeron Chair", "Kursi ergonomis premium untuk WFH.", 24999000m),
        };

        foreach (var (name, desc, price) in homeGarden)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                Stock = random.Next(5, 50),
                ImageUrl = $"https://placehold.co/400x400/2d3748/fff?text={Uri.EscapeDataString(name.Split(' ')[0])}",
                SellerId = sellerId,
                CategoryId = 4,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
            });
        }

        // Sports (CategoryId = 5)
        var sports = new[]
        {
            ("Garmin Fenix 7X Solar", "Smartwatch adventure dengan solar charging dan peta TopoActive.", 14999000m),
            ("Yonex Astrox 99 Pro Badminton", "Raket badminton pro dengan teknologi Rotational Generator System.", 2999000m),
            ("Peloton Bike+", "Sepeda statis dengan layar rotating 23.8 inci.", 39999000m),
            ("TRX Pro4 Suspension Trainer", "Alat latihan suspension training profesional.", 3499000m),
            ("Theragun Pro Massage Gun", "Alat pijat terapi untuk recovery otot.", 8999000m),
            ("Wilson Pro Staff 97 Tennis", "Raket tenis signature Roger Federer.", 3999000m),
            ("Bowflex SelectTech Dumbbells", "Dumbbell adjustable 5-52.5 lbs.", 7999000m),
            ("Manduka PRO Yoga Mat", "Matras yoga premium dengan ketebalan 6mm.", 1799000m),
            ("Whoop 4.0 Fitness Tracker", "Wearable untuk tracking sleep dan recovery.", 4999000m),
            ("Callaway Paradym Driver", "Driver golf dengan teknologi Jailbreak AI.", 8999000m),
            ("Hydrow Rowing Machine", "Mesin rowing dengan layar 22 inci dan live classes.", 29999000m),
            ("Fitbit Charge 6", "Fitness tracker dengan GPS dan ECG.", 2699000m),
            ("Nike Metcon 8 Training", "Sepatu training untuk CrossFit dan gym.", 1999000m),
            ("Osprey Atmos AG 65 Backpack", "Tas hiking dengan Anti-Gravity suspension.", 4999000m),
            ("Black Diamond Climbing Harness", "Harness climbing untuk indoor dan outdoor.", 1299000m),
            ("Speedo Fastskin Pure Focus Mirror", "Kacamata renang kompetisi anti-fog.", 999000m),
            ("Suunto 9 Peak Pro", "GPS watch ultra-tipis untuk endurance sports.", 9999000m),
            ("Hyperice Hypervolt 2 Pro", "Percussion massage device untuk athlete.", 5999000m),
            ("Trek Domane SL 5 Road Bike", "Sepeda road endurance dengan IsoSpeed decoupler.", 35999000m),
            ("Salomon Speedcross 6 Trail", "Sepatu trail running dengan grip Contagrip.", 2199000m),
        };

        foreach (var (name, desc, price) in sports)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                Stock = random.Next(10, 80),
                ImageUrl = $"https://placehold.co/400x400/276749/fff?text={Uri.EscapeDataString(name.Split(' ')[0])}",
                SellerId = sellerId,
                CategoryId = 5,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
            });
        }

        // Add more electronics to reach 100
        var moreElectronics = new[]
        {
            ("Razer BlackWidow V4 Pro", "Keyboard gaming mekanik dengan RGB Chroma.", 3799000m),
            ("SteelSeries Arctis Nova Pro", "Headset gaming dengan ANC dan dual wireless.", 5499000m),
            ("Elgato Stream Deck MK.2", "Kontroler streaming dengan 15 tombol LCD.", 2499000m),
            ("Sony PlayStation 5", "Konsol gaming next-gen dengan SSD ultra cepat.", 8999000m),
            ("Xbox Series X", "Konsol gaming Microsoft dengan 12 teraflops GPU.", 8499000m),
            ("Rode NT-USB+ Microphone", "Mikrofon USB studio-quality untuk content creator.", 2699000m),
            ("Elgato Key Light Air", "Panel LED untuk streaming dan video conference.", 2299000m),
            ("Logitech StreamCam", "Webcam Full HD 60fps untuk streaming.", 2499000m),
            ("Synology DS220+ NAS", "Network Attached Storage 2-bay untuk backup.", 5999000m),
            ("TP-Link Deco XE75 WiFi 6E", "Mesh WiFi system dengan tri-band 5400Mbps.", 4999000m),
            ("Samsung T7 Shield 2TB SSD", "Portable SSD tahan air dan tahan jatuh.", 2999000m),
            ("Seagate IronWolf 8TB NAS HDD", "Hard drive untuk NAS dengan 7200 RPM.", 3499000m),
            ("Crucial P5 Plus 2TB NVMe", "SSD NVMe PCIe 4.0 dengan speed 6600MB/s.", 2799000m),
            ("ASUS ROG Swift PG32UQX", "Monitor gaming 4K 144Hz dengan Mini LED.", 44999000m),
            ("Corsair Vengeance DDR5 32GB", "RAM DDR5 dengan speed 5600MHz untuk gaming.", 2299000m),
        };

        foreach (var (name, desc, price) in moreElectronics)
        {
            products.Add(new Product
            {
                Name = name,
                Description = desc,
                Price = price,
                Stock = random.Next(10, 60),
                ImageUrl = $"https://placehold.co/400x400/1e40af/fff?text={Uri.EscapeDataString(name.Split(' ')[0])}",
                SellerId = sellerId,
                CategoryId = 1,
                IsActive = true,
                CreatedAt = DateTime.Now.AddDays(-random.Next(1, 60))
            });
        }

        dbContext.Products.AddRange(products);
        await dbContext.SaveChangesAsync();
    }
}
