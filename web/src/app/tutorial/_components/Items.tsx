"use client";

import { useState } from "react";
import Image, { StaticImageData } from "next/image";
import { ChevronLeft, ChevronRight, LucideIcon } from "lucide-react";
import { clsx } from "@/app/utils/clsx";

export const Items = ({
  items,
}: {
  items: {
    image: StaticImageData | string;
    title: string;
    description: string;
  }[];
}) => {
  const [selectedItemIndex, setSelectedItemIndex] = useState(0);

  const handlePrevious = () => {
    setSelectedItemIndex((prev) => (prev === 0 ? items.length - 1 : prev - 1));
  };

  const handleNext = () => {
    setSelectedItemIndex((prev) => (prev === items.length - 1 ? 0 : prev + 1));
  };

  return (
    <div className="flex-1 w-full flex flex-col pb-8 pt-4 gap-10">
      <div className="overflow-hidden rounded-lg w-full flex-1 flex flex-col">
        <a
          href="/"
          className="font-bold underline hover:underline text-center bg-white/60 py-2 rounded-lg shadow-md"
        >
          실시간 대시보드 보러가기
        </a>
        <div
          className="flex transition-transform duration-300 ease-in-out flex-1 items-center"
          style={{ transform: `translateX(-${selectedItemIndex * 100}%)` }}
        >
          {items.map((item, index) => (
            <div
              key={[item.title, index].join("-")}
              className="w-full flex-shrink-0"
            >
              <div className="relative flex flex-col w-full gap-6">
                <div className="relative w-full aspect-[16/9] overflow-hidden rounded-lg">
                  {typeof item.image !== "string" ? (
                    <Image
                      src={item.image}
                      alt={item.title}
                      fill
                      className="object-cover"
                      priority
                    />
                  ) : (
                    <img
                      src={item.image}
                      alt={item.title}
                      className="object-cover"
                    />
                  )}
                </div>
                <div className="flex flex-col items-center bg-white/70 shadow-md px-3 py-4 gap-6 rounded-lg">
                  <h3 className="text-xl font-semibold">{item.title}</h3>
                  <p className="whitespace-pre-line text-center text-sm break-keep">
                    {item.description}
                  </p>
                </div>
              </div>
            </div>
          ))}
        </div>
        <div className="flex space-x-2 justify-center">
          {items.map((item, index) => (
            <button
              key={[item.title, index].join("-")}
              type="button"
              onClick={() => setSelectedItemIndex(index)}
              className={clsx(
                "h-2 w-2 rounded-full transition-colors",
                selectedItemIndex === index ? "bg-black" : "bg-gray-300",
              )}
            />
          ))}
        </div>
      </div>

      {/* Navigation and indicator dots */}
      <div className="mt-4 flex items-center justify-center gap-2 -mx-2">
        <Button icon={ChevronLeft} onClick={handlePrevious} />
        <Button icon={ChevronRight} onClick={handleNext} />
      </div>
    </div>
  );
};

type ButtonProps = {
  icon: LucideIcon;
  onClick: () => void;
  className?: string;
};

const Button = ({ icon: Icon, onClick, className }: ButtonProps) => {
  return (
    <button
      type="button"
      onClick={onClick}
      className={clsx(
        "flex-1 flex items-center justify-center rounded-lg bg-white/70 shadow-md hover:bg-white py-3 active:bg-white",
        className,
      )}
    >
      <Icon className="h-5 w-5" />
    </button>
  );
};
