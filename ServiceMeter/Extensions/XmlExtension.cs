/*
 * MIT License
 *
 * Copyright (c) Evgeny Nazarchuk.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */

using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ServiceMeter;

public static class XmlExtension
{
    public static string FromObjectToXmlString<T>(
        this T value,
        XmlWriterSettings xmlSettings)
        where T : class
    {
        var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
        var serializer = new XmlSerializer(typeof(T));
        using var stringWriter = new StringWriter();
        using var xmlWriter = XmlWriter.Create(stringWriter, xmlSettings);

        serializer.Serialize(xmlWriter, value, namespaces);

        return stringWriter.ToString();
    }

    public static T? FromXmlStringToObject<T>(this string value)
        where T : class
    {
        using var reader = new StringReader(value);
        var serializer = new XmlSerializer(typeof(T));
        return serializer.Deserialize(reader) as T;
    }
}
